using UnityEngine;
using UnityEngine.UI;

public class UIContentManager : MonoBehaviour {

    Text _panelStartLevelValue, _panelResultLevelValue, _levelResultText, _resultLevelButtonText, 
        _panelStart_PointsLivingTime;
    GameObject _levelStartPanel, _levelResultPanel;
    public GameMode_1 GameMode_1;

    public void Start()
    {
        _levelStartPanel = GameObject.Find("Panel_Start");
        _levelResultPanel = GameObject.Find("Panel_Result");
        _panelResultLevelValue = GameObject.Find("PanelResultLevelValue").GetComponent<Text>();
        _panelStartLevelValue = GameObject.Find("PanelStartLevelValue").GetComponent<Text>();
        _levelResultText = GameObject.Find("LevelResult_text").GetComponent<Text>();
        _resultLevelButtonText = GameObject.Find("PanelResult_ButtonRun_Text").GetComponent<Text>();
        _panelStart_PointsLivingTime = GameObject.Find("PanelStart_PointsLivingTime").GetComponent<Text>();
        _levelResultPanel.SetActive(false);
    }

    public void OpenLevelStartPanel()
    {
        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
            GameMode_1.CurrentLevel = GameMode_1.GameLevels[GameMode_1.CurrentLevel.LevelNo + 1];

        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _levelResultPanel.SetActive(false);
        _levelStartPanel.SetActive(true);
        LoadPanelsWithData();
    }

    public void ActivateResultPanel()
    {
        _levelResultPanel.SetActive(true);
        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win) {
            _levelResultText.color = new Color32(50, 160, 50, 255);
            _levelResultText.text = "Passed !!";
            _resultLevelButtonText.text = "NEXT LEVEL";
        } else
        {
            _levelResultText.color = Color.red;
            _levelResultText.text = "Lost";
            _resultLevelButtonText.text = "TRY AGAIN";
        }
    }

    public void LoadPanelsWithData()
    {
        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelStart_PointsLivingTime.text = GameMode_1.CurrentLevel.PointsLivingTimer.Lenght.ToString() + " sec";
    }
}
