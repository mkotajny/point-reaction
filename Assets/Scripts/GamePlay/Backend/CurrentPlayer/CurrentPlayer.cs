 using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using Firebase.Database;

public static class CurrentPlayer
{
    static string _playerId, _playerName;
    static bool _signedIn;
    static CampaignItem _campaignItem;
    static int _livesTaken;

    public static string PlayerId { get { return _playerId; } }
    public static string PlayerName { get { return _playerName; } }
    public static bool SignedIn { get { return _signedIn; } }

    public static CampaignItem CampaignItem
    {
        get { return _campaignItem; }
        set { _campaignItem = value; }
    }
    public static int LivesTaken
    {
        get { return _livesTaken; }
        set { _livesTaken = value; }
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
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = _playerName;

    }


    public static void GetCurrentPlayerData()
    {
        _livesTaken = 0;
        _campaignItem = new CampaignItem(System.DateTime.Now.ToString("yyyy-MM-dd"), _playerId, _playerName, 1, 0, 0, 30, 0, 0);
        if (!CheckInternet.IsConnected()) return;
        FirebasePR.CampaignDbReference
            .OrderByChild("PlayerId")
            .EqualTo(_playerId)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        _campaignItem = new CampaignItem(childSnapshot.Child("Updated").Value.ToString()
                                , childSnapshot.Child("PlayerId").Value.ToString()
                                , childSnapshot.Child("PlayerName").Value.ToString()
                                , System.Convert.ToInt32(childSnapshot.Child("LvlNo").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("LvlMiles").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("HitsCmp").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("Lives").Value)
                                , System.Convert.ToInt32(childSnapshot.Child("Ads").Value)
                                , System.Convert.ToDouble(childSnapshot.Child("ReacCmp").Value));
                        return;
                    }
                }
            });
    }
}
 