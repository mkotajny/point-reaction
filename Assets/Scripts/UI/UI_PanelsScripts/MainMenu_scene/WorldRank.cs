using UnityEngine;
using UnityEngine.UI;

public class WorldRank : MonoBehaviour {

    public GameObject GridWorldRank;
    GameObject _worldRankItemPrefab, _worldRankItemObject;
    bool _worldRankLoaded;
    
    void Awake()
    {
        _worldRankItemPrefab = Resources.Load("Panel_WorldRank_Item") as GameObject;
    }

    void OnEnable()
    {
        for (int i = 0; i < GridWorldRank.transform.childCount; i++)
            Destroy(GridWorldRank.transform.GetChild(i).gameObject);

        _worldRankLoaded = false;
        WorldRankPersister.LoadWorldRank();
    }

    private void Update()
    {
        if (!_worldRankLoaded && WorldRankPersister.WorldRank.Count > 0)
        {
            int counter = 1;
            foreach (WorldRankItem worldRankItem in WorldRankPersister.WorldRank)
            {
                _worldRankItemObject = Instantiate(_worldRankItemPrefab);
                _worldRankItemObject.transform.SetParent(GridWorldRank.transform, false);
                _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().text = (counter).ToString() + ".";
                _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().text = worldRankItem.PlayerName;
                _worldRankItemObject.transform.GetChild(2).GetComponent<Text>().text =
                    "Level: " + worldRankItem.LevelNo + "." + worldRankItem.PointsHit
                    + "\nAvg Reaction: " + worldRankItem.ReactionAvg + "s";
                counter++;
            }
            _worldRankLoaded = true;
        }
    }
}
