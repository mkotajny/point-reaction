using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class UiControler : MonoBehaviour {

    public Text PlayerName;


    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ActivityLogger.InitializeLog();

        PlayerName = GameObject.Find("PlayerName_background").GetComponent<Text>();
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text
        = "Version " + Application.version;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        if (PlayerPrefs.GetInt("InGooglePlay") == 1)
            GooglePlaySignIn();
    }


    public void GooglePlaySignIn()
    {
        Social.localUser.Authenticate(success =>
        {
            PlayerName.text = Social.localUser.userName;
            PlayerPrefs.SetInt("InGooglePlay", 1);
        });
    }

    public void GooglePlaySignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        PlayerName.text = string.Empty;
        PlayerPrefs.SetInt("InGooglePlay", 0);
    }
}
