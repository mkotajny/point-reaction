using UnityEngine;
using UnityEngine.UI;


public class SceneControler : MonoBehaviour {


    private void Awake()
    {
        MusicPR.PlayNextSong(MusicPR.PlayListMenu);
        MusicPR.SetVolumeSfx();
    }

    void OnEnable()
    {
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        if (CurrentPlayer.CampaignItem != null)
            GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.CampaignItem.PlrName;

        FirebasePR.InitializeFireBaseDb();
        if (PlayerPrefs.GetInt("InGooglePlay") == 1 && !Social.localUser.authenticated)
            CurrentPlayer.SignInGooglePlay();

        if (CurrentPlayer.LivesTaken > 0)
        {
            CurrentPlayer.CampaignItem.Lives = CurrentPlayer.CampaignItem.Lives - CurrentPlayer.LivesTaken;
            CurrentPlayer.LivesTaken = 0;
        }
    }

    void Update()
    {
        if (MusicPR.NextSongTimer.TimeElapsed())
            MusicPR.PlayNextSong(MusicPR.PlayListMenu);
    }

    private void OnDestroy()
    {
        FirebasePR.SignOutFireBase();
    }

}