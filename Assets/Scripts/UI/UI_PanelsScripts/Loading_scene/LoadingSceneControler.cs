using UnityEngine;

public class LoadingSceneControler : MonoBehaviour {

    public GameObject _progressBarPanel;

    private void Awake()
    {
        int progressStages = 1;
        WorldRankPersister.Reset();
        _progressBarPanel = GameObject.Find("Panel_InProgress");

        if (CheckInternet.IsConnected())
        {
            FirebasePR.InitializeFireBaseDb();
            progressStages += 1;
            if (PlayerPrefs.GetInt("InGooglePlay") == 1)
            {
                progressStages += 4;
                CurrentPlayer.SignInGooglePlay();
            }
        }
        ProgressBarPR.Activate("Loading ...", progressStages);

        if (progressStages == 1)
            ProgressBarPR.AddProgress("No internet game load");    
    }

    void Update () {
        if (ProgressBarPR.ProgressStatus == ProgressBarPrStatuses.Succeded
            || (ProgressBarPR.ProgressStatus == ProgressBarPrStatuses.Failed
                && !_progressBarPanel.activeInHierarchy))
        {
            ProgressBarPR.Deactivate();
            Initiate.Fade("MainMenuScene", Color.black, 1.0f);
        }
    }
}
