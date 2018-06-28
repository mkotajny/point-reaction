using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class MainMenu : MonoBehaviour {


    void Awake ()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ActivityLogger.InitializeLog();

        GameObject.Find("VersionNumber_text").GetComponent<Text>().text
        = "Version " + Application.version;


        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Debug.Log("debug: Sign in");
        Social.localUser.Authenticate(success => {
            GameObject.Find("PlayerName_text").GetComponent<Text>().text 
            = "Player: " + Social.localUser.userName + ".";
            Debug.Log("debug: Sign in succeded");
        });

        /*
        try
        {

            PlayGamesPlatform.Instance.SignOut();
            Debug.Log("debug: Sign out succeded");
        }
        catch
        {
            Debug.Log("debug: Sign out failed");
        }

        Social.localUser.Authenticate(success => {
            Debug.Log("debug: signet in again");
        });
        */

    }

    void OnEnable()
    {
        //ActivityLogger.AddLogLine("MAIN MENU panel has been opened");
    }

}
