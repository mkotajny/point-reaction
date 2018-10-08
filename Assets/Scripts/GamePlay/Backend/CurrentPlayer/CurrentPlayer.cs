using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;

public static class CurrentPlayer
{
    static bool _signedIn, _trialMode;
    static CampaignItem _campaignItem;
    static CampaignsHistoryItem _campaignsHistoryItem;
    static WorldRankItem _worldRankItem;    
    static int _livesTaken;
    static bool _bonusInformed;

    public static bool SignedIn { get { return _signedIn; } }

    public static CampaignItem CampaignItem
    {
        get { return _campaignItem; }
        set { _campaignItem = value; }
    }
    public static CampaignsHistoryItem CampaignsHistoryItem
    {
        get { return _campaignsHistoryItem; }
        set { _campaignsHistoryItem = value; }
    }
    public static WorldRankItem WorldRankItem
    {
        get { return _worldRankItem; }
        set { _worldRankItem = value; }
    }
    public static int LivesTaken
    {
        get { return _livesTaken; }
        set { _livesTaken = value; }
    }
    public static bool BonusInformed
    {
        get { return _bonusInformed; }
        set { _bonusInformed = value; }
    }
    public static bool TrialMode
    {
        get { return _trialMode; }
        set { _trialMode = value; }
    }


    public static void SignInGooglePlay()
    {
        if (!CheckInternet.IsConnected()) return;

        FirebasePR.InitializeFireBaseDb();
        FirebasePR.InitializeGooglePlay();
        AdMobPR.Initialize();

        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                Debug.LogError("debug: SignInOnClick: Failed to Sign into Play Games Services.");
                return;
            }

            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                Debug.LogError("debug: SignInOnClick: Signed into Play Games Services but failed to get the server auth code.");
                return;
            }
            Debug.LogFormat("debug: SignInOnClick: Auth code is: {0}", authCode);

            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
            FirebasePR.FirebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("debug: SignInOnClick was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("debug: SignInOnClick encountered an error: " + task.Exception);
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result;
                
                Debug.LogFormat("debug: SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                AdMobPR.RequestRewardBasedVideo();
                GetCurrentPlayerData(newUser.UserId);
                SetPlayerAttributes(Social.localUser.userName);

                PlayerPrefs.SetInt("InGooglePlay", 1);
                _signedIn = true;
            });
        });
    }

    public static void SignOutGooglePlay()
    {
        FirebasePR.FirebaseAuth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        _campaignItem.PlrId = string.Empty;
        _campaignItem.PlrName = string.Empty;
        _signedIn = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }

    static void SetPlayerAttributes(string userName)
    {
        _campaignItem.PlrName = userName;
        _bonusInformed = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = _campaignItem.PlrName;

    }

    public static void GetCurrentPlayerData(string userId)
    {
        _livesTaken = 0;
        _trialMode = false;
        if (!CheckInternet.IsConnected()) return;
        GetCampaignData(userId);
        GetCampaignsHistoryData(userId);
        GetWorldRankData(userId);
    }

    public static void SetTrialMode()
    {
        _trialMode = true;
        _campaignItem = new CampaignItem(string.Empty, string.Empty, 1, 0, 10, 0, 0, 0);
    }

    static void GetCampaignData(string playerId)
    {
        _campaignItem = new CampaignItem(playerId, string.Empty, 1, 0, 30, 0, 0, 0);

        FirebasePR.CampaignDbReference
            .OrderByChild("PlrId")
            .EqualTo(playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _campaignItem.Updated = childSnapshot.Child("Updated").Value.ToString();
                        _campaignItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _campaignItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                        _campaignItem.LvlNo = System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value);
                        _campaignItem.HitsCmp = System.Convert.ToInt32(childSnapshot.Child("HitsCmp").Value);
                        _campaignItem.Lives = System.Convert.ToInt32(childSnapshot.Child("Lives").Value);
                        _campaignItem.Ads = System.Convert.ToInt32(childSnapshot.Child("Ads").Value);
                        _campaignItem.ReacCmp = System.Convert.ToDouble(childSnapshot.Child("ReacCmp").Value);
                        _campaignItem.BnsTaken = System.Convert.ToInt32(childSnapshot.Child("BnsTaken").Value);
                    }
                }
                return;
            });
    }

    static void GetCampaignsHistoryData(string playerId)
    {
        _campaignsHistoryItem = new CampaignsHistoryItem(playerId, string.Empty, 0, 0, 0, 0);

        FirebasePR.CampaignsHistoryDbReference
            .OrderByChild("PlrId")
            .EqualTo(playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _campaignsHistoryItem.UpdDt = childSnapshot.Child("UpdDt").Value.ToString();
                        _campaignsHistoryItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _campaignsHistoryItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                        _campaignsHistoryItem.Cmpgns = System.Convert.ToInt32(childSnapshot.Child("Cmpgns").Value);
                        _campaignsHistoryItem.AdsWtchd = System.Convert.ToInt32(childSnapshot.Child("AdsWtchd").Value);
                        _campaignsHistoryItem.AdsSkpd = System.Convert.ToInt32(childSnapshot.Child("AdsSkpd").Value);
                        _campaignsHistoryItem.BnsBtnInf = System.Convert.ToInt32(childSnapshot.Child("BnsBtnInf").Value);
                    }
                }
                return;
            });
    }

    static void GetWorldRankData(string playerId)
    {
        _worldRankItem = new WorldRankItem(playerId, string.Empty, 1, 0, 0);

        FirebasePR.WorldRankDbReference
            .OrderByChild("PlrId")
            .EqualTo(playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _worldRankItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _worldRankItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                        _worldRankItem.LvlNo = System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value)       ;
                        _worldRankItem.PtsHit = System.Convert.ToInt32(childSnapshot.Child("PtsHit").Value);
                        _worldRankItem.ReacAvg = System.Convert.ToDouble(childSnapshot.Child("ReacAvg").Value);
                        _worldRankItem.CalculateFinalPoints();
                    }
                }
                return;
            });
    }

}
