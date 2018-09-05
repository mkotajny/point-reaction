using UnityEngine;
using UnityEngine.UI;

public class UIContentManager : MonoBehaviour {

    public Text _panelResultLevelValue, _levelResultQtyPointsHit, _levelResultReactionAvg, _levelResultText, _resultLevelButtonText;

    Text _panelStartLevelValue, _panelStart_PointsLivingTime;
    public ZUIManager _zuiManager;
    Button _backToMainMenuButton;
    public GameObject[] backgrounds, victoryAnimations;
    int _selectedBackgorundIndex, _selectedVictoryAnimationIndex;
    System.Random _randomizer = new System.Random();


    GameObject _levelStartPanel, _levelResultPanel;
    public GameMode_1 GameMode_1;

    public void Start()
    {
        _levelStartPanel = GameObject.Find("Panel_Start");
        _panelStartLevelValue = GameObject.Find("PanelStartLevelValue").GetComponent<Text>();
        _panelStart_PointsLivingTime = GameObject.Find("PanelStart_PointsLivingTime").GetComponent<Text>();
        _levelResultPanel = GameObject.Find("Panel_Result");
        _backToMainMenuButton = GameObject.Find("ButtonBlue_Back").GetComponent<Button>();
    }

    public void OpenLevelStartPanel()
    {
        victoryAnimations[_selectedVictoryAnimationIndex].SetActive(false);

        if (GameMode_1.CurrentLevel.HitsQty == 10)
            GameMode_1.LevelUp();

        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        try { _zuiManager.OpenMenu("Menu_Start"); } catch { }

        _selectedBackgorundIndex = _randomizer.Next(0, 5);
        backgrounds[_selectedBackgorundIndex].SetActive(true);

        LoadPanelsWithData();
    }


    public void ActivateResultPanel()
    {
        _zuiManager.OpenMenu("Menu_Result");

        LoadPanelsWithData();
        ActivityLogger.SaveLog();
        _backToMainMenuButton.gameObject.SetActive(true);
        _selectedVictoryAnimationIndex = _randomizer.Next(0, 8);
        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
            victoryAnimations[_selectedVictoryAnimationIndex].SetActive(true);
    }

    public void DeacTivateBackgroundAnimation()
    {
        backgrounds[_selectedBackgorundIndex].SetActive(false);
    }

    public void LoadPanelsWithData()
    {
        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelStart_PointsLivingTime.text = (GameMode_1.CurrentLevel.PointsLivingTimer.Lenght).ToString() + " sec";
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _levelResultQtyPointsHit.text = GameMode_1.CurrentLevel.HitsQty.ToString();

        if (GameMode_1.CurrentLevel.HitsQty > 0) 
            _levelResultReactionAvg.text = GameMode_1.CurrentLevel.ReactionAvg.ToString("0.00") + " sec";
        else 
            _levelResultReactionAvg.text = "-";

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
