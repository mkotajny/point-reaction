 using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;

public static class CurrentPlayer
{
    static string _playerId, _playerName;
    static bool _signedIn;
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

                SetPlayerAttributes(newUser.UserId, Social.localUser.userName);
                GetCurrentPlayerData();

                PlayerPrefs.SetInt("InGooglePlay", 1);
                _signedIn = true;
            });
        });
    }

    public static void SignOutGooglePlay()
    {
        FirebasePR.FirebaseAuth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        _playerId = null;
        _playerName = string.Empty;
        _signedIn = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }

    static void SetPlayerAttributes(string userId, string userName)
    {
        _playerId = userId;
        _playerName = userName;
        _bonusInformed = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = _playerName;

    }

    public static void GetCurrentPlayerData()
    {
        _livesTaken = 0;
        if (!CheckInternet.IsConnected()) return;
        GetCampaignData();
        GetCampaignsHistoryData();
        GetWorldRankData();
    }

    static void GetCampaignData()
    {
        _campaignItem = new CampaignItem(_playerId, _playerName, 1, 0, 30, 0, 0, 0);

        FirebasePR.CampaignDbReference
            .OrderByChild("PlrId")
            .EqualTo(_playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _campaignItem.Updated = childSnapshot.Child("Updated").Value.ToString();
                        _campaignItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _campaignItem.PlrName = _playerName;
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

    static void GetCampaignsHistoryData()
    {
        _campaignsHistoryItem = new CampaignsHistoryItem(_playerId, _playerName, 0, 0, 0, 0);

        FirebasePR.CampaignsHistoryDbReference
            .OrderByChild("PlrId")
            .EqualTo(_playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _campaignsHistoryItem.UpdDt = childSnapshot.Child("UpdDt").Value.ToString();
                        _campaignsHistoryItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _campaignsHistoryItem.PlrName = _playerName;
                        _campaignsHistoryItem.Cmpgns = System.Convert.ToInt32(childSnapshot.Child("Cmpgns").Value);
                        _campaignsHistoryItem.AdsWtchd = System.Convert.ToInt32(childSnapshot.Child("AdsWtchd").Value);
                        _campaignsHistoryItem.AdsSkpd = System.Convert.ToInt32(childSnapshot.Child("AdsSkpd").Value);
                        _campaignsHistoryItem.BnsBtnInf = System.Convert.ToInt32(childSnapshot.Child("BnsBtnInf").Value);
                    }
                }
                return;
            });
    }

    static void GetWorldRankData()
    {
        _worldRankItem = new WorldRankItem(_playerId, _playerName, 1, 0, 0);

        FirebasePR.WorldRankDbReference
            .OrderByChild("PlrId")
            .EqualTo(_playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _worldRankItem.PlrId = childSnapshot.Child("PlrId").Value.ToString();
                        _worldRankItem.PlrName = _playerName;
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
