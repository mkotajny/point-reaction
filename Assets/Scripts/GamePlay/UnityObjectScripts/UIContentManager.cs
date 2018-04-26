using UnityEngine;
using UnityEngine.UI;

public class UIContentManager : MonoBehaviour {

    Text _panelStartLevelValue, _panelStart_PointsLivingTime;
    Text _panelResultLevelValue, _levelResultQtyPointsHit, _levelResultReactionAvg, _levelResultReactionFastest
        ,  _levelResultText, _resultLevelButtonText;

    GameObject _levelStartPanel, _levelResultPanel;
    public GameMode_1 GameMode_1;

    public void Start()
    {
        _levelStartPanel = GameObject.Find("Panel_Start");
        _panelStartLevelValue = GameObject.Find("PanelStartLevelValue").GetComponent<Text>();
        _panelStart_PointsLivingTime = GameObject.Find("PanelStart_PointsLivingTime").GetComponent<Text>();

        _levelResultPanel = GameObject.Find("Panel_Result");
        _panelResultLevelValue = GameObject.Find("PanelResultLevelValue").GetComponent<Text>();
        _levelResultQtyPointsHit = GameObject.Find("QtyOfPointsHit_value").GetComponent<Text>();
        _levelResultReactionAvg = GameObject.Find("AvgReaction_value").GetComponent<Text>();
        _levelResultReactionFastest = GameObject.Find("FastestReaction_value").GetComponent<Text>();

        _levelResultText = GameObject.Find("LevelResult_value").GetComponent<Text>();
        _resultLevelButtonText = GameObject.Find("PanelResult_ButtonRun_Text").GetComponent<Text>();


        _levelResultPanel.SetActive(false);
    }

    public void OpenLevelStartPanel()
    {
        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
            GameMode_1.LevelUp();

        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        LoadPanelsWithData();
        _levelResultPanel.SetActive(false);
        _levelStartPanel.SetActive(true);
    }

    public void ActivateResultPanel()
    {
        LoadPanelsWithData();
        _levelResultPanel.SetActive(true);
    }

    public void LoadPanelsWithData()
    {
        _panelStartLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _panelStart_PointsLivingTime.text = GameMode_1.CurrentLevel.PointsLivingTimer.Lenght.ToString() + " sec";
        _panelResultLevelValue.text = GameMode_1.CurrentLevel.LevelNo.ToString();
        _levelResultQtyPointsHit.text = GameMode_1.CurrentLevel.HitsQty.ToString();
        _levelResultReactionAvg.text = GameMode_1.CurrentLevel.ReactionAvg.ToString("0.00") + " sec";
        _levelResultReactionFastest.text = GameMode_1.CurrentLevel.ReactionFastest.ToString("0.00") + " sec";

        if (GameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
        {
            _levelResultText.color = new Color32(50, 160, 50, 255);
            _levelResultText.text = "( Passed )";
            _resultLevelButtonText.text = "NEXT LEVEL";
        }
        else
        {
            _levelResultText.color = Color.red;
            _levelResultText.text = "( Not passed )";
            _resultLevelButtonText.text = "TRY AGAIN";
        }
    }
}
