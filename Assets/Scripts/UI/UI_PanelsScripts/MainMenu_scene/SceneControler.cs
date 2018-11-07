using UnityEngine;
using UnityEngine.UI;

public class SceneControler : MonoBehaviour {


    private void Awake()
    {
        SessionVariables.SetSession();
        if (SessionVariables.TrialMode)
            SessionVariables.EndTrialMode();

        MusicPR.PlayNextSong(MusicPR.PlayListMenu);
        MusicPR.SetVolumeSfx();
    }

    void OnEnable()
    {

        GameObject.Find("VersionNumber_text").GetComponent<Text>().text = "Version " + Application.version;
        if (CurrentPlayer.CampaignItem != null)
            GameObject.Find("PlayerName_background").GetComponent<Text>().text = CurrentPlayer.CampaignItem.PlrName;

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

        if (!FirebasePR.FirebaseInitialized
            && CheckInternet.IsConnected())
            FirebasePR.InitializeFireBaseDb();
    }
}