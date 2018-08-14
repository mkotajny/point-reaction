using UnityEngine;
using UnityEngine.UI;


public class SceneControler : MonoBehaviour {

    Timer _checkOnlineTimer;

    void OnEnable()
    {
        _checkOnlineTimer = new Timer(3);
        _checkOnlineTimer.Activate();
        ActivityLogger.InitializeLog();

        Screen.orientation = ScreenOrientation.Portrait;
        GameLevelPersister.LevelLoad();
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.PlayerName;

        if (PlayerPrefs.GetInt("InGooglePlay") == 1 && !Social.localUser.authenticated)
            CurrentPlayer.SignInGooglePlay();
        FirebasePR.InitializeFireBaseDb();
    }

    void Update()
    {
        if (_checkOnlineTimer.TimeElapsed() && Social.localUser.authenticated)
        {
            WorldRankPersister.UpdateCurrentPlayer();
            _checkOnlineTimer.Activate();
        }
    }

}