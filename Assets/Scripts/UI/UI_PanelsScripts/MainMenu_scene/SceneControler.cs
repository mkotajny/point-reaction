using UnityEngine;
using UnityEngine.UI;


public class SceneControler : MonoBehaviour {

    Timer _checkOnlineTimer;

    void Awake()
    {
        _checkOnlineTimer = new Timer(3);
        ActivityLogger.InitializeLog();
    }

    void OnEnable()
    {
        UpdateCurrentPlayer();

        Screen.orientation = ScreenOrientation.Portrait;
        GameLevelPersister.LevelLoad();
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.PlayerName;
        FirebasePR.InitializeFireBaseDb();
        if (PlayerPrefs.GetInt("InGooglePlay") == 1 && !Social.localUser.authenticated)
            CurrentPlayer.SignInGooglePlay();
    }

    void Update()
    {
        if (_checkOnlineTimer.TimeElapsed())
            UpdateCurrentPlayer();
    }

    void UpdateCurrentPlayer()
    {
        if (Social.localUser.authenticated)
            WorldRankPersister.UpdateCurrentPlayer();
        _checkOnlineTimer.Activate();
    }
}