using UnityEngine;
using UnityEngine.UI;
using Afrenchguy;
using System.Collections.Generic;

public class UIContentManager : MonoBehaviour {

    Text _panelStartLevel, _panelStart_PointsLivingTime, _panelStartHitsToWin, _panelStartMissesToLoose, _getBonusButtonText;
    Button _backToMainMenuButton;
    public Button GetBonusButton;
    BonusPanelScriptContainer _bonusPanel;
    int _selectedVictoryAnimationIndex;
    List<TextListItem> _notificationBarItems = new List<TextListItem>();
    

    public Text _panelResultLevelValue, _levelResultText, _resultLevelButtonText, _panelResult_PerfectBonus, _panelResult_AvgReaction;

    public ZUIManager _zuiManager;
    public GameObject[] backgrounds, victoryAnimations;
    public UicmUpperPanel UpperPanel;
    public Menu MenuStart;
    public GameObject _perfectBonusImage;
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
        GetBonusButton = GameObject.Find("ButtonBlue_GetBonus").GetComponent<Button>();
        _getBonusButtonText = GameObject.Find("ButtonBlue_GetBonus_Text").GetComponent<Text>();
        UpperPanel = GameObject.Find("UicmUpperPanel").GetComponent<UicmUpperPanel>();
        _zuiManager.CurActiveMenu = MenuStart;
        _notificationBarItems.Add(new TextListItem("Personal best !\n(world rank updated)", BicepsSprite));
    }

    private void Update()
    {

        if (AdMobPR.AdmobPRSatuses == AdmobPRSatuses.AdClosedAfterReward)
        {
            AdMobPR.AdmobPRSatuses = AdmobPRSatuses.AdNotStarted;
            _bonusPanel.ActivatePanel(this);
        }

    }

    public void OpenLevelStartPanel()
    {
        victoryAnimations[_selectedVictoryAnimationIndex].SetActive(false);

        // open level start panel
        GameMode_1.CurrentLevel.Reset();
        UpperPanel.SetUpperPanelStats(GameMode_1.CurrentLevel);
        LoadPanelsWithData();
        try { _zuiManager.OpenMenu("Menu_Start"); } catch { }
        backgrounds[0].SetActive(true);

        if (CurrentPlayer.CampaignItem.Lives <= 0)  // open game over panel
        {
            _backToMainMenuButton.gameObject.SetActive(false);
            backgrounds[3].SetActive(true);
            GameMode_1.GameOver();
            try { _zuiManager.OpenMenu("Menu_GameOver"); } catch { }
            return;
        }

        GetBonusButton.gameObject.SetActive(false);

        if (CurrentPlayer.CampaignItem.BnsTaken < CurrentPlayer.CampaignItem.BonusesAvailable())  //activate ad --> bonus
        {
            _getBonusButtonText.text = (CurrentPlayer.CampaignItem.BonusesAvailable() - CurrentPlayer.CampaignItem.BnsTaken == 1)
                ? "Bonus": (CurrentPlayer.CampaignItem.BonusesAvailable() - CurrentPlayer.CampaignItem.BnsTaken).ToString() + " bonuses";

            GetBonusButton.gameObject.SetActive(true);
        }
    }

    public void ActivateResultPanel(bool debug = false)
    {
        _zuiManager.OpenMenu("Menu_Result");
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
        if (AdMobPR.RewardBasedVideo.IsLoaded())
            AdMobPR.ShowRewardBasedVideo();
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
        else
        {
            _levelResultText.color = Color.red;
            _levelResultText.text = " Not passed";
            _resultLevelButtonText.text = "Try Again";
        }
    }
}
