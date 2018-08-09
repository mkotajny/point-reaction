using UnityEngine;
using UnityEngine.UI;


public class BestResults : MonoBehaviour {

    void OnEnable()
    {
        string _bestLevelNo, _pointsHit, _reactionAvg, _reactionFastest;

        _bestLevelNo = GameLevelPersister.BestLevelNoPersistence.ToString();
        _pointsHit = GameLevelPersister.LevelPersistence.HitsQty.ToString();
        _reactionAvg = GameLevelPersister.LevelPersistence.ReactionAvg.ToString() + " sec";
        _reactionFastest = GameLevelPersister.LevelPersistence.ReactionFastest.ToString() + " sec";

        if (_pointsHit == "0")
        {
            _pointsHit = "-";
            _reactionAvg = "-";
            _reactionFastest = "-";
        }

        GameObject.Find("PanelResult_LevelAchieved").GetComponent<Text>().text = _bestLevelNo;
        GameObject.Find("PanelResult_PointsHit").GetComponent<Text>().text = _pointsHit;
        GameObject.Find("PanelResult_AvgReaction").GetComponent<Text>().text = _reactionAvg;
        GameObject.Find("PanelResult_FastestReaction").GetComponent<Text>().text = _reactionFastest;
    }
}
