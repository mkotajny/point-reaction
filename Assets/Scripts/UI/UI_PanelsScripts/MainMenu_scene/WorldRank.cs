using UnityEngine;
using UnityEngine.UI;

public class WorldRank : MonoBehaviour {

    public GameObject GridWorldRank, PanelWorldRank, PanelMainMenu;
    GameObject _worldRankItemPrefab, _worldRankItemObject;
    bool _worldRankRendered;
    
    void Awake()
    {
        if (WorldRankPersister.WorldRank != null)
            WorldRankPersister.Reset();

        _worldRankItemPrefab = Resources.Load("Panel_WorldRank_Item") as GameObject;
    }

    void OnEnable()
    {
        for (int i = 0; i < GridWorldRank.transform.childCount; i++)
            Destroy(GridWorldRank.transform.GetChild(i).gameObject);
        _worldRankRendered = false;        
    }

    private void Update()
    {
        if (FirebasePR.WorldRankDbReference != null
            && WorldRankPersister.WorldRank.Count == 0
            && !WorldRankPersister.LoadInProgress)
            WorldRankPersister.LoadWorldRank();

        if (!_worldRankRendered 
            && WorldRankPersister.WorldRank.Count > 0
            && !WorldRankPersister.LoadInProgress)
        {
            int counter = 1;
            foreach (WorldRankItem worldRankItem in WorldRankPersister.WorldRank)
            {
                _worldRankItemObject = Instantiate(_worldRankItemPrefab);
                _worldRankItemObject.transform.SetParent(GridWorldRank.transform, false);
                _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().text = (counter).ToString() + ".";
                _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().text = worldRankItem.PlrName;
                _worldRankItemObject.transform.GetChild(2).GetComponent<Text>().text =
                    "Level: " + worldRankItem.LvlNo + "." + worldRankItem.PtsHit
                    + "\nAvg Reaction: " + worldRankItem.ReacAvg + "s";

                if (CurrentPlayer.CampaignItem != null
                        && worldRankItem.PlrName == CurrentPlayer.CampaignItem.PlrName)
                {
                    _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                    _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                }
                counter++;
            }
            _worldRankRendered = true;
        }
    }
}
