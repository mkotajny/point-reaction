using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;


public class SceneControler : MonoBehaviour {


    private void Awake()
    {
        MusicPR.PlayNextSong(MusicPR.PlayListMenu);
        MusicPR.SetVolumeSfx();
    }

    void OnEnable()
    {
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.PlayerName;

        FirebasePR.InitializeFireBaseDb();
        if (PlayerPrefs.GetInt("InGooglePlay") == 1 && !Social.localUser.authenticated)
            CurrentPlayer.SignInGooglePlay();
    }

    void Update()
    {
        if (MusicPR.NextSongTimer.TimeElapsed())
            MusicPR.PlayNextSong(MusicPR.PlayListMenu);
    }

}