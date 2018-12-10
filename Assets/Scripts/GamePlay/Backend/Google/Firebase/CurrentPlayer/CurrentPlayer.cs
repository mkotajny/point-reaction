using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;
using System.Threading;

public static class CurrentPlayer
{
    public static bool SignedIn { get; private set; }
    public static CampaignItem CampaignItem { get; set; }
    public static CampaignsHistoryItem CampaignsHistoryItem { get; set; }
    public static WorldRankItem WorldRankItem { get; set; }
    public static int LivesTaken { get; set; }
    public static bool BonusInformed { get; set; }

    public static void SignInGooglePlay()
    {
        try
        {
            if (!CheckInternet.IsConnected()) return;

            FirebasePR.InitializeGooglePlay();

            Social.localUser.Authenticate((bool success) =>
            {
                if (!success)
                {
                    Debug.LogError("debug: SignInOnClick: Failed to Sign into Play Games Services.");
                    ProgressBarPR.SetFail("Failed to Sign into Play Games Services.");
                    return;
                }
               
                string authCode = GetAuthCode();
                if (string.IsNullOrEmpty(authCode)) return;

                Debug.LogFormat("debug: SignInOnClick: Auth code is: {0}", authCode);

                Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
                FirebasePR.FirebaseAuth.SignInWithCredentialAsync(credential).ContinueWith((System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> task) =>
                {
                    ProgressBarPR.AddProgress("signed with credentials");
                    if (task.IsCanceled)
                    {
                        Debug.LogError("debug: SignInOnClick was canceled.");
                        ProgressBarPR.SetFail("SignInOnClick was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("debug: SignInOnClick encountered an error: " + task.Exception);
                        ProgressBarPR.SetFail("SignInOnClick encountered an system exception.");
                        return;
                    }
                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    Debug.LogFormat("debug: SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                    FirebasePR.CampaignDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns/" + newUser.UserId);
                    FirebasePR.CampaignsHistoryDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns_history/" + newUser.UserId);
                    GetCurrentPlayerData(newUser.UserId, Social.localUser.userName);
                    SetPlayerAttributes(Social.localUser.userName);
                    PlayerPrefs.SetInt("InGooglePlay", 1);
                    SignedIn = true;
                });
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("debug: " + e.Message + "::: " + e.StackTrace);
        }
    }

    static string GetAuthCode()
    {
        int i;
        string authCode = string.Empty;
        bool firstAttemptFailed = false;
        for (i = 0; i < 5; i++)
        {
            authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                if (i == 0) firstAttemptFailed = true;
                Debug.LogError("debug: SignInOn"
                    + i + ": Signed into Play Games Services but failed to get the server auth code.");
                Thread.Sleep(1500);
            } else break;
        }
        if (firstAttemptFailed)
        {
            if (string.IsNullOrEmpty(authCode))
            {
                Debug.LogError("debug: All 5 attemts of getting auth code failed");
                SessionVariables.ActivityLog.Send(LogCategories.GooglePlayAuthCodeFailed
                    , "All 5 attemts of getting auth code failed");
                ProgressBarPR.SetFail("Failed to get google play auth code.");
            } else SessionVariables.ActivityLog.Send(LogCategories.GooglePlayAuthCodeFailed
                , "Auth code gathered afer attempt no " + (i+1).ToString());
        }
        return authCode;
    }

    public static void SignOutGooglePlay()
    {
        FirebasePR.FirebaseAuth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        CampaignItem.PlrId = string.Empty;
        CampaignItem.PlrName = string.Empty;
        SignedIn = false;
        WorldRankPersister.CurrentPlayerPosition = 0;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }

    static void SetPlayerAttributes(string userName)
    {
        CampaignItem.PlrName = userName;
        BonusInformed = false;
        try { GameObject.Find("PlayerName_background").GetComponent<Text>().text = CampaignItem.PlrName; } catch { }
    }

    public static void GetCurrentPlayerData(string userId, string userName)
    {
        LivesTaken = 0;
        SessionVariables.TrialMode = false;
        if (!CheckInternet.IsConnected()) return;
        GetCampaignData(userId, userName);
        GetCampaignsHistoryData(userId, userName);
        GetWorldRankData(userId, userName);
    }

    static void GetCampaignData(string playerId, string userName)
    {
        CampaignItem = new CampaignItem(playerId, userName, 1, 0, 10, 0, 0, 0, SessionVariables.CurrentGameVersion.VersionString);
        AdMobPR.Initialize();
        FirebasePR.CampaignDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                    {
                        CampaignItem.Updated = snapshot.Child("Updated").Value.ToString();
                        CampaignItem.PlrId = snapshot.Child("PlrId").Value.ToString();
                        CampaignItem.PlrName = snapshot.Child("PlrName").Value.ToString();
                        CampaignItem.LvlNo = System.Convert.ToInt32(snapshot.Child("LvlNo").Value);
                        CampaignItem.HitsCmp = System.Convert.ToInt32(snapshot.Child("HitsCmp").Value);
                        CampaignItem.Lives = System.Convert.ToInt32(snapshot.Child("Lives").Value);
                        CampaignItem.ReacCmp = System.Convert.ToDouble(snapshot.Child("ReacCmp").Value);
                        CampaignItem.BnsTaken = System.Convert.ToInt32(snapshot.Child("BnsTaken").Value);
                        CampaignItem.BnsLastMlstn = System.Convert.ToInt32(snapshot.Child("BnsLastMlstn").Value);
                        SessionVariables.ActivityLog = new ActivityLogIem();
                    }
                    ProgressBarPR.AddProgress("get campaign data");
                }
                return;
            });
    }

    static void GetCampaignsHistoryData(string playerId, string userName)
    {
        CampaignsHistoryItem = new CampaignsHistoryItem(playerId, userName, 0, 0, 0, SessionVariables.CurrentGameVersion.VersionString);

        FirebasePR.CampaignsHistoryDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                    {
                        CampaignsHistoryItem.UpdDt = snapshot.Child("UpdDt").Value.ToString();
                        CampaignsHistoryItem.PlrId = snapshot.Child("PlrId").Value.ToString();
                        CampaignsHistoryItem.PlrName = snapshot.Child("PlrName").Value.ToString();
                        CampaignsHistoryItem.Cmpgns = System.Convert.ToInt32(snapshot.Child("Cmpgns").Value);
                        CampaignsHistoryItem.AdsWtchd = System.Convert.ToInt32(snapshot.Child("AdsWtchd").Value);
                        CampaignsHistoryItem.AdsSkpd = System.Convert.ToInt32(snapshot.Child("AdsSkpd").Value);
                    }
                    ProgressBarPR.AddProgress("get campaign history data");
                }
                return;
            });
    }

    static void GetWorldRankData(string playerId, string userName)
    {
        WorldRankItem = new WorldRankItem(playerId, userName, 1, 0, 0);
        FirebasePR.WorldRankDbReference
            .OrderByChild("PlrId")
            .EqualTo(playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                    {
                        foreach (var childSnapshot in snapshot.Children)
                        {
                            WorldRankItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                            WorldRankItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                            WorldRankItem.LvlNo = System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value);
                            WorldRankItem.PtsHit = System.Convert.ToInt32(childSnapshot.Child("PtsHit").Value);
                            WorldRankItem.ReacAvg = System.Convert.ToDouble(childSnapshot.Child("ReacAvg").Value);
                            WorldRankItem.CalculateFinalPoints();
                            ProgressBarPR.AddProgress("get players World Rank data");
                            WorldRankPersister.LoadWorldRank();  // as worldRankItem.ReacAvg is required for calculating Player's position in the world rank
                        }
                    }
                    else
                    {
                        ProgressBarPR.AddProgress("no players World Rank data");
                        WorldRankPersister.LoadWorldRank();
                    }
                }
                return;
            });
    }

}
