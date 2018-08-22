using UnityEngine;
using UnityEngine.UI;


public class SceneControler : MonoBehaviour {

    Timer _checkOnlineTimer;

    private void Awake()
    {
        try
        {
            ActivityLogger.InitializeLog();
            GameLevelPersister.LevelLoad();
            MusicPR.PlayNextSong(MusicPR.PlayListMenu);
            MusicPR.SetVolumeSfx();
        }
        catch (System.Exception e)
        {
            Debug.LogError("debug: " + e.Message + "::: " + e.StackTrace);
        }
    }

    void OnEnable()
    {
        _checkOnlineTimer = new Timer(3);
        _checkOnlineTimer.Activate();

        Screen.orientation = ScreenOrientation.Portrait;
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
        if (MusicPR.NextSongTimer.TimeElapsed())
            MusicPR.PlayNextSong(MusicPR.PlayListMenu);
    }

}