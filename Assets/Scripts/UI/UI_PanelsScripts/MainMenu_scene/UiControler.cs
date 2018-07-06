using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;




public class UiControler : MonoBehaviour {

    public Text PlayerName;
    Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ActivityLogger.InitializeLog();

        PlayerName = GameObject.Find("PlayerName_background").GetComponent<Text>();
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text
        = "Version " + Application.version;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
         .RequestServerAuthCode(false /*forceRefresh*/)
         .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Debug.LogFormat("debug: SignInOnClick: Play Games Configuration initialized");

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        if (PlayerPrefs.GetInt("InGooglePlay") == 1)
            FireBaseGooglePlaySignIn();
    }


    public void FireBaseGooglePlaySignIn()
    {
        // Sign In and Get a server auth code.
        UnityEngine.Social.localUser.Authenticate((bool success) =>
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

            // Use Server Auth Code to make a credential
            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            // Sign In to Firebase with the credential
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
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
                PlayerName.text = newUser.DisplayName;
                PlayerPrefs.SetInt("InGooglePlay", 1);
            });
        });
    }

    public void GooglePlaySignOut()
    {
        auth.SignOut();
        PlayGamesPlatform.Instance.SignOut();
        PlayerName.text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }
}
