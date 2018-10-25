using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;

public static class CurrentPlayer
{
    static bool _signedIn, _trialMode;
    static CampaignItem _campaignItem, _campaignItemCopy;
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

        FirebasePR.InitializeGooglePlay();
        AdMobPR.Initialize();

        Social.localUser.Authenticate((bool success) =>
        {
            ProgressBarPR.AddProgress("authentication start");
            if (!success)
            {
                Debug.LogError("debug: SignInOnClick: Failed to Sign into Play Games Services.");
                ProgressBarPR.SetFail("Failed to Sign into Play Games Services.");
                return;
            }

            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                Debug.LogError("debug: SignInOnClick: Signed into Play Games Services but failed to get the server auth code.");
                ProgressBarPR.SetFail("Failed to get google play auth code.");
                return;
            }
            Debug.LogFormat("debug: SignInOnClick: Auth code is: {0}", authCode);

            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
            FirebasePR.FirebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
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

                AdMobPR.RequestRewardBasedVideo();
                FirebasePR.CampaignDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns/" + newUser.UserId);
                FirebasePR.CampaignsHistoryDbReference = FirebaseDatabase.DefaultInstance.GetReference("campaigns_history/" + newUser.UserId);
                FirebasePR.WorldRankDbReference = FirebaseDatabase.DefaultInstance.GetReference("world_rank");
                GetCurrentPlayerData(newUser.UserId, Social.localUser.userName);
                SetPlayerAttributes(Social.localUser.userName);
                WorldRankPersister.LoadWorldRank();
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
        try { GameObject.Find("PlayerName_background").GetComponent<Text>().text = _campaignItem.PlrName; } catch { }
    }

    public static void GetCurrentPlayerData(string userId, string userName)
    {
        _livesTaken = 0;
        _trialMode = false;
        if (!CheckInternet.IsConnected()) return;
        GetCampaignData(userId, userName);
        GetCampaignsHistoryData(userId, userName);
        GetWorldRankData(userId, userName);
    }

    public static void SetTrialMode()
    {
        if (_campaignItem != null)
            _campaignItemCopy = new CampaignItem(_campaignItem.PlrId, _campaignItem.PlrName, _campaignItem.LvlNo, _campaignItem.HitsCmp
                , _campaignItem.Lives, _campaignItem.Ads, _campaignItem.ReacCmp, _campaignItem.BnsTaken, _campaignItem.BnsLastMlstn);
        _campaignItem = new CampaignItem(string.Empty, string.Empty, 1, 0, 10, 0, 0, 0, 0);
        _trialMode = true;
    }
    public static void EndTrialMode()
    {
        if (_campaignItemCopy != null)
            _campaignItem = new CampaignItem(_campaignItemCopy.PlrId, _campaignItemCopy.PlrName, _campaignItemCopy.LvlNo, _campaignItemCopy.HitsCmp
            , _campaignItemCopy.Lives, _campaignItemCopy.Ads, _campaignItemCopy.ReacCmp, _campaignItemCopy.BnsTaken, _campaignItemCopy.BnsLastMlstn);
        _trialMode = false;
    }

    static void GetCampaignData(string playerId, string userName)
    {
        _campaignItem = new CampaignItem(playerId, userName, 1, 0, 30, 0, 0, 0, 0);
        FirebasePR.CampaignDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                    {
                        _campaignItem.Updated = snapshot.Child("Updated").Value.ToString();
                        _campaignItem.PlrId = snapshot.Child("PlrId").Value.ToString();
                        _campaignItem.PlrName = snapshot.Child("PlrName").Value.ToString();
                        _campaignItem.LvlNo = System.Convert.ToInt32(snapshot.Child("LvlNo").Value);
                        _campaignItem.HitsCmp = System.Convert.ToInt32(snapshot.Child("HitsCmp").Value);
                        _campaignItem.Lives = System.Convert.ToInt32(snapshot.Child("Lives").Value);
                        _campaignItem.Ads = System.Convert.ToInt32(snapshot.Child("Ads").Value);
                        _campaignItem.ReacCmp = System.Convert.ToDouble(snapshot.Child("ReacCmp").Value);
                        _campaignItem.BnsTaken = System.Convert.ToInt32(snapshot.Child("BnsTaken").Value);
                        _campaignItem.BnsLastMlstn = System.Convert.ToInt32(snapshot.Child("BnsLastMlstn").Value);
                    }
                    ProgressBarPR.AddProgress("get campaign data");
                }
                return;
            });
    }

    static void GetCampaignsHistoryData(string playerId, string userName)
    {
        _campaignsHistoryItem = new CampaignsHistoryItem(playerId, userName, 0, 0, 0, 0);

        FirebasePR.CampaignsHistoryDbReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                    {
                        _campaignsHistoryItem.UpdDt = snapshot.Child("UpdDt").Value.ToString();
                        _campaignsHistoryItem.PlrId = snapshot.Child("PlrId").Value.ToString();
                        _campaignsHistoryItem.PlrName = snapshot.Child("PlrName").Value.ToString();
                        _campaignsHistoryItem.Cmpgns = System.Convert.ToInt32(snapshot.Child("Cmpgns").Value);
                        _campaignsHistoryItem.AdsWtchd = System.Convert.ToInt32(snapshot.Child("AdsWtchd").Value);
                        _campaignsHistoryItem.AdsSkpd = System.Convert.ToInt32(snapshot.Child("AdsSkpd").Value);
                        _campaignsHistoryItem.BnsBtnInf = System.Convert.ToInt32(snapshot.Child("BnsBtnInf").Value);
                    }
                    ProgressBarPR.AddProgress("get campaign history data");
                }
                return;
            });
    }

    static void GetWorldRankData(string playerId, string userName)
    {
        _worldRankItem = new WorldRankItem(playerId, userName, 1, 0, 0);

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
                            _worldRankItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                            _worldRankItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                            _worldRankItem.LvlNo = System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value);
                            _worldRankItem.PtsHit = System.Convert.ToInt32(childSnapshot.Child("PtsHit").Value);
                            _worldRankItem.ReacAvg = System.Convert.ToDouble(childSnapshot.Child("ReacAvg").Value);
                            _worldRankItem.CalculateFinalPoints();
                            ProgressBarPR.AddProgress("get players World Rank data");
                        }
                    } else ProgressBarPR.AddProgress("get players World Rank data");
                }
                return;
            });
    }
}
