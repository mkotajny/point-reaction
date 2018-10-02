 using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;

public static class CurrentPlayer
{
    static string _playerId, _playerName;
    static bool _signedIn;
    static CampaignItem _campaignItem;
    static WorldRankItem _worldRankItem;
    static int _livesTaken;
    static bool _bonusProposed;

    public static bool SignedIn { get { return _signedIn; } }

    public static CampaignItem CampaignItem
    {
        get { return _campaignItem; }
        set { _campaignItem = value; }
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
    public static bool BonusProposed {
        get { return _bonusProposed; }
        set { _bonusProposed = value; }
    }


    public static void SignInGooglePlay()
    {
        if (!CheckInternet.IsConnected()) return;

        FirebasePR.InitializeGooglePlay();

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

                SetPlayerAttributes(newUser.UserId, newUser.DisplayName);
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
        _bonusProposed = false;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = _playerName;

    }

    public static void GetCurrentPlayerData()
    {
        _livesTaken = 0;
        if (!CheckInternet.IsConnected()) return;
        GetCampaignData();
        GetWorldRankData();
    }

    static void GetCampaignData()
    {
        _campaignItem = new CampaignItem(System.DateTime.Now.ToString("yyyy-MM-dd"), _playerId, _playerName, 1, 0, 30, 0, 0, 0);

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
                        _campaignItem.PlrName = childSnapshot.Child("PlrName").Value.ToString();
                        _campaignItem.LvlNo = System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value);
                        _campaignItem.HitsCmp = System.Convert.ToInt32(childSnapshot.Child("HitsCmp").Value);
                        _campaignItem.Lives = System.Convert.ToInt32(childSnapshot.Child("Lives").Value);
                        _campaignItem.Ads = System.Convert.ToInt32(childSnapshot.Child("Ads").Value);
                        _campaignItem.ReacCmp = System.Convert.ToDouble(childSnapshot.Child("ReacCmp").Value);
                        _campaignItem.BnsTaken = System.Convert.ToInt32(childSnapshot.Child("BnsTaken").Value);
                    }
                }
                if (_campaignItem.Lives == 0)
                    _campaignItem.ResetCampaign();
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
