using UnityEngine;
using UnityEngine.UI;
using Afrenchguy;
using System.Collections.Generic;

public class UIContentManager : MonoBehaviour {

    Text _panelStartLevel, _panelStart_PointsLivingTime, _panelStartHitsToWin, _panelStartMissesToLoose, _getBonusButtonText;
    Button _backToMainMenuButton;
    BonusPanelScriptContainer _bonusPanel;
    int _selectedVictoryAnimationIndex;
    List<TextListItem> _notificationBarItems = new List<TextListItem>();
    

    public Text _panelResultLevelValue, _levelResultText, _resultLevelButtonText, _panelResult_PerfectBonus, _panelResult_AvgReaction;
    public Button GetBonusButton;
    public ZUIManager ZuiManager;
    public GameObject[] backgrounds, victoryAnimations;
    public UicmUpperPanel UpperPanel;
    public Menu MenuStart;
    public GameObject _perfectBonusImage, GetBonusButtonEffects;
    public GameMode_1 GameMode_1;
    public System.Random Randomizer = new System.Random();
    public Sprite BicepsSprite;
    public TextsListAnimation NotificationBarAnimation;

    public void Awake()
    {
        _bonusPanel = GameObject.Find("BonusPanelScriptContainer").GetComponent<BonusPanelScriptContainer>();
        _panelStartLevel = GameObject.Find("PanelStartLevelValue").GetComponent<Text>();
        _panelStart_PointsLivingTime = GameObject.Find("PanelStart_PointsLivingTime").GetComponent<Text>();
        _panelStartHitsToWin = GameObject.Find("Panel_HitsToWin_value").GetComponent<Text>();
        _panelStartMissesToLoose = GameObject.Find("Panel_MistakesToLoose_value").GetComponent<Text>();
        _backToMainMenuButton = GameObject.Find("ButtonBlue_Back").GetComponent<Button>();
        _getBonusButtonText = GameObject.Find("ButtonBlue_GetBonus_Text").GetComponent<Text>();
        _notificationBarItems.Add(new TextListItem("Personal best !\n(world rank updated)", BicepsSprite));
        GetBonusButton = GameObject.Find("ButtonBlue_GetBonus").GetComponent<Button>();
        UpperPanel = GameObject.Find("UicmUpperPanel").GetComponent<UicmUpperPanel>();
        ZuiManager.CurActiveMenu = MenuStart;
}

    private void Update()
    {

        if (AdMobPR.AdmobPRSatuses == AdmobPRSatuses.AdClosedAfterReward)
        {
            AdMobPR.AdmobPRSatuses = AdmobPRSatuses.StartingPoint;
            _bonusPanel.ActivatePanel(this);
        }

        if (GetBonusButton.IsActive())
        {
            if (!GetBonusButtonEffects.activeInHierarchy && (AdMobPR.RewardBasedVideo.IsLoaded() || AdMobPR.LoadingAddAttempts > 1))
            {
                GetBonusButtonEffects.SetActive(true);
                Debug.Log("debug: UiContentManager-->Update: chk1: bonus button effects activated");
            }
            if (GetBonusButtonEffects.activeInHierarchy && !AdMobPR.RewardBasedVideo.IsLoaded() && AdMobPR.LoadingAddAttempts <= 1)
            {
                GetBonusButtonEffects.SetActive(false);
                Debug.Log("debug: UiContentManager-->Update: chk2: bonus button effects deactivated");
            }
        }

        if (AdMobPR.LoadingAdTimer != null && AdMobPR.LoadingAdTimer.TimeElapsed())
        {
            if (AdMobPR.LoadingAddAttempts < 3)
            {
                Debug.Log("debug: 30 seconds of loading Advertisement elapsed, attempt no " + (AdMobPR.LoadingAddAttempts + 1).ToString() + " started.");
                AdMobPR.RequestRewardBasedVideo();
            } else
            {
                SessionVariables.ActivityLog.Send(LogCategories.ThreeAdLoadsFailed, "Three attempts of loading Advert. failed");
                AdMobPR.LoadingAdTimer.Deactivate();
            }
        }
    }

    public void OpenLevelStartPanel()
    {
        victoryAnimations[_selectedVictoryAnimationIndex].SetActive(false);
        GetBonusButton.gameObject.SetActive(false);

        // open game over panel
        if (CurrentPlayer.CampaignItem.Lives <= 0)  
        {
            GetBonusButton.gameObject.SetActive(false);
            _backToMainMenuButton.gameObject.SetActive(false);
            backgrounds[3].SetActive(true);
            try { ZuiManager.OpenMenu("Menu_GameOver"); } catch { }
            return;
        }

        // open (OFFLINE) game over panel
        if (SessionVariables.TrialMode
            && CurrentPlayer.CampaignItem.LvlNo == 11)
        {
            GetBonusButton.gameObject.SetActive(false);
            _backToMainMenuButton.gameObject.SetActive(false);
            try { ZuiManager.OpenMenu("Menu_End_Of_Trial"); } catch { }
            return;
        }

        //activate ad --> bonus button
        if (CurrentPlayer.CampaignItem.BnsTaken < CurrentPlayer.CampaignItem.BonusesAvailable()
            && !CurrentPlayer.CampaignItem.BonusTakenInCurrentMilestone())
            GetBonusButton.gameObject.SetActive(true);

        // open "info about bonus" panel
        if (!SessionVariables.TrialMode
            && (CurrentPlayer.CampaignItem.LvlNo - 1) % 5 == 0         // milestone level
            && (CurrentPlayer.CampaignItem.LvlNo - 1) / 5 == 1      //1st milestone
            && (CurrentPlayer.CampaignsHistoryItem.BnsBtnInf < 2)   // informed not more then 2 times in campaigns history
            && !CurrentPlayer.BonusInformed)                        // not already informed in current session
        {
            try { ZuiManager.OpenMenu("Menu_Info_About_Bonus"); } catch { }
            CurrentPlayer.BonusInformed = true;
            CurrentPlayer.CampaignsHistoryItem.BnsBtnInf++;
            CurrentPlayer.CampaignsHistoryItem.SaveToFirebase();
            return;
        }

        // open level start panel
        GameMode_1.CurrentLevel.Reset();
        UpperPanel.SetUpperPanelStats(GameMode_1.CurrentLevel);
        try { ZuiManager.OpenMenu("Menu_Start"); } catch { }
        LoadPanelsWithData();
        backgrounds[0].SetActive(true);
    }

    public void ActivateResultPanel(bool debug = false)
    {
        ZuiManager.OpenMenu("Menu_Result");
        LoadPanelsWithData();
        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
        {
            _selectedVictoryAnimationIndex = Randomizer.Next(0, 8);
            victoryAnimations[_selectedVictoryAnimationIndex].SetActive(true);
            GameMode_1.LevelUp();
        }
        GameMode_1.SaveToFireBase(false, this);
        _backToMainMenuButton.gameObject.SetActive(true);
    }

    public void ShowPersonalBestNotification()
    {
        NotificationBarAnimation.Init(_notificationBarItems);
    }

    public void DeacTivateBackgroundAnimation()
    {
        backgrounds[0].SetActive(false);
    }

    public void GetBonus()
    {
        if (!SessionVariables.TrialMode && AdMobPR.RewardBasedVideo.IsLoaded())
        {
            AdMobPR.ShowRewardBasedVideo();
            return;
        }
        if (SessionVariables.TrialMode || AdMobPR.LoadingAddAttempts > 1)
        {
            CurrentPlayer.CampaignItem.BnsTaken++;
            if (AdMobPR.LoadingAddAttempts > 1)
                SessionVariables.ActivityLog.Send(LogCategories.BonusWithoudAdvert, "Player gets bonus without fail-loaded advert");
            if (!SessionVariables.TrialMode) AdMobPR.RequestRewardBasedVideo(restartAttempts: true);
            
            _bonusPanel.ActivatePanel(this);
            return;
        }
        if (AdMobPR.LoadingAddAttempts == 1) ZuiManager.OpenMenu("Menu_LoadAdInProgress");
    }

    void LoadPanelsWithData()
    {
        _panelStartLevel.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelStart_PointsLivingTime.text = (GameMode_1.CurrentLevel.PointsLivingTimer.Lenght).ToString() + " sec";
        _panelStartHitsToWin.text = GameMode_1.CurrentLevel.HitsToWin.ToString();
        _panelStartMissesToLoose.text = (GameMode_1.CurrentLevel.MissesToLoose < CurrentPlayer.CampaignItem.Lives ? 
            GameMode_1.CurrentLevel.MissesToLoose : CurrentPlayer.CampaignItem.Lives).ToString();
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelResult_PerfectBonus.text = GameMode_1.CurrentLevel.BonusPerfectLevel.ToString();

        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win
            && GameMode_1.CurrentLevel.MissQty == 0)
        {
            _perfectBonusImage.SetActive(true);
            _panelResult_PerfectBonus.text =
                GameMode_1.CurrentLevel.BonusPerfectLevel.ToString();
            if (GameMode_1.CurrentLevel.BonusPerfectLevel <= 10)
                _panelResult_PerfectBonus.text = "+" + _panelResult_PerfectBonus.text;
        }
        else
        {
            _perfectBonusImage.SetActive(false);
            _panelResult_PerfectBonus.text = "-";
        }

        if (CurrentPlayer.CampaignItem.HitsCmp > 0)
            _panelResult_AvgReaction.text = (CurrentPlayer.CampaignItem.ReacCmp / CurrentPlayer.CampaignItem.HitsCmp).ToString("0.00");

        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
        {
            _levelResultText.color = new Color32(50, 160, 50, 255);
            _levelResultText.text = " PASSED !";
            _resultLevelButtonText.text = "Next Level";
        }
        else if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Lost)
        {
            _levelResultText.color = Color.red;
            _levelResultText.text = " Not passed";
            _resultLevelButtonText.text = "Try Again";
        }
    }
}
