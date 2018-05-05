using UnityEngine;
using UnityEngine.UI;


public class BestResults : MonoBehaviour {

    public void Awake()
    {
        GameObject.Find("PanelResult_LevelAchieved").GetComponent<Text>().text 
            = PlayerPrefs.GetString("LevelNo");
        GameObject.Find("PanelResult_PointsHit").GetComponent<Text>().text
            = PlayerPrefs.GetString("PointsHit");
        GameObject.Find("PanelResult_AvgReaction").GetComponent<Text>().text 
            = PlayerPrefs.GetString("ReactionAvg");
        GameObject.Find("PanelResult_FastestReaction").GetComponent<Text>().text 
            = PlayerPrefs.GetString("ReactionFastest");
    }
}
