using UnityEngine;
using UnityEngine.UI;

public class UIContentManager : MonoBehaviour {

    Text _panelStartLevel, _panelStart_PointsLivingTime, _panelStartHitsToWin, _panelStartMissesToLoose;
    Button _backToMainMenuButton;
    BonusPanel _bonusPanel;
    int _selectedVictoryAnimationIndex;
    

    public Text _panelResultLevelValue, _levelResultText, _resultLevelButtonText, _panelResult_PerfectBonus, _panelResult_AvgReaction;
    public ZUIManager _zuiManager;
    public GameObject[] backgrounds, victoryAnimations;
    public UicmUpperPanel UpperPanel;
    public Menu MenuStart;
    
    public GameObject _perfectBonusImage;
    public GameMode_1 GameMode_1;
    public System.Random Randomizer = new System.Random();

    public void Awake()
    {
        _panelStartLevel = GameObject.Find("PanelStartLevelValue").GetComponent<Text>();
        _panelStart_PointsLivingTime = GameObject.Find("PanelStart_PointsLivingTime").GetComponent<Text>();
        _panelStartHitsToWin = GameObject.Find("Panel_HitsToWin_value").GetComponent<Text>();
        _panelStartMissesToLoose = GameObject.Find("Panel_MistakesToLoose_value").GetComponent<Text>();
        _backToMainMenuButton = GameObject.Find("ButtonBlue_Back").GetComponent<Button>();
        UpperPanel = GameObject.Find("UicmUpperPanel").GetComponent<UicmUpperPanel>();
        _bonusPanel = GameObject.Find("BonusPanel").GetComponent<BonusPanel>();
        _zuiManager.CurActiveMenu = MenuStart;
    }

    public void OpenLevelStartPanel()
    {
        victoryAnimations[_selectedVictoryAnimationIndex].SetActive(false);

        if (CurrentPlayer.CampaignItem.Lives <= 0)  // open game over panel
        {
            _backToMainMenuButton.gameObject.SetActive(false);
            backgrounds[3].SetActive(true);
            CurrentPlayer.CampaignItem.ResetCampaign();
            try { _zuiManager.OpenMenu("Menu_GameOver"); } catch { }
            return;
        }

        if (GameMode_1.CurrentLevel.LevelNo > 2   // open milestone bonus panel
            && GameMode_1.GameLevels[GameMode_1.CurrentLevel.LevelNo - 2].BonusMileStoneLevel > 0
            && CurrentPlayer.CampaignItem.LvlMilest != GameMode_1.CurrentLevel.LevelNo - 1)
        {
            _bonusPanel.ActivatePanel(this);
            return;
        }

        // open level start panel
        GameMode_1.CurrentLevel.Reset();
        UpperPanel.SetUpperPanelStats(GameMode_1.CurrentLevel);
        LoadPanelsWithData();
        try { _zuiManager.OpenMenu("Menu_Start"); } catch { }
        backgrounds[0].SetActive(true);
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
        GameMode_1.SaveToFireBase();
        _backToMainMenuButton.gameObject.SetActive(true);
    }

    public void DeacTivateBackgroundAnimation()
    {
        backgrounds[0].SetActive(false);
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
