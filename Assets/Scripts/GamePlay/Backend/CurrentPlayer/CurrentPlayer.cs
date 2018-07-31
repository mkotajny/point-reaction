using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public static class CurrentPlayer
{
    static Text _playerNameUnity;
    static string _playerId, _playerName;
    static bool _signedIn;
    public static string PlayerId { get { return _playerId; } }
    public static string PlayerName { get { return _playerName; } }
    public static bool SignedIn { get { return _signedIn; } }

    public static void SignInFireBase()
    {
        if (_playerNameUnity == null)
            _playerNameUnity = GameObject.Find("PlayerName_background").GetComponent<Text>();

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
                _playerId = newUser.UserId;
                _playerName = newUser.DisplayName;
                _signedIn = true;
                _playerNameUnity.text = _playerName;
                PlayerPrefs.SetInt("InGooglePlay", 1);
            });
        });
    }

    public static void SignOutFireBase()
    {
        FirebasePR.FirebaseAuth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        _playerId = null;
        _playerName = string.Empty;
        _signedIn = false;
        _playerNameUnity.text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }

}