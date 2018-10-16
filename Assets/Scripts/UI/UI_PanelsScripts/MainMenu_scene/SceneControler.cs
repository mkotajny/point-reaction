using UnityEngine;
using UnityEngine.UI;


public class SceneControler : MonoBehaviour {


    private void Awake()
    {
        if (CurrentPlayer.TrialMode)
            CurrentPlayer.EndTrialMode();

        MusicPR.PlayNextSong(MusicPR.PlayListMenu);
        MusicPR.SetVolumeSfx();

    }

    void OnEnable()
    {
        //PlayerPrefs.SetInt("InGooglePlay", 0);

        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        if (CurrentPlayer.CampaignItem != null)
            GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.CampaignItem.PlrName;

        FirebasePR.InitializeFireBaseDb();

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
}