using UnityEngine;

public class LoadingSceneControler : MonoBehaviour {

    public GameObject _progressBarPanel;

    private void Awake()
    {
        SessionVariables.SetSession();
        _progressBarPanel = GameObject.Find("Panel_InProgress");
        int progressStages = 1;
        WorldRankPersister.Reset();

        if (CheckInternet.IsConnected())
        {
            progressStages += 2;
            if (PlayerPrefs.GetInt("InGooglePlay") == 1)
                progressStages += 4;

            FirebasePR.InitializeFireBaseDb();
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
