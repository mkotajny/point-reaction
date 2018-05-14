using UnityEngine;
using UnityEngine.UI;


public class BestResults : MonoBehaviour {

    void Awake()
    {
        int _currentLevelNo, _bestLevelNo;
        _currentLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("LevelNo"), 0);
        _bestLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("BestLevelNo"), -1);
        if (_bestLevelNo == -1)
            _bestLevelNo = _currentLevelNo;

        GameObject.Find("PanelResult_LevelAchieved").GetComponent<Text>().text 
            = _bestLevelNo.ToString();
        GameObject.Find("PanelResult_PointsHit").GetComponent<Text>().text
            = PlayerPrefs.GetString("PointsHit");
        GameObject.Find("PanelResult_AvgReaction").GetComponent<Text>().text 
            = PlayerPrefs.GetString("ReactionAvg");
        GameObject.Find("PanelResult_FastestReaction").GetComponent<Text>().text 
            = PlayerPrefs.GetString("ReactionFastest");
    }

    void OnEnable()
    {
        ActivityLogger.AddLogLine("PLAYER BEST RESULTS panel has been opened");
    }

}
