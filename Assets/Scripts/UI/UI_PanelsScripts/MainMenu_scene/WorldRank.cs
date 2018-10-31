using UnityEngine;
using UnityEngine.UI;

public class WorldRank : MonoBehaviour {

    public GameObject GridWorldRank, GridScrollPanel, CurrentPlayerPositionPanel, CurrentPlayerPositionItem;
    GameObject _worldRankItemPrefab, _worldRankItemObject;
    bool _worldRankRendered, _worldRankWindowExtended;

    void Awake()
    {
        _worldRankItemPrefab = Resources.Load("Panel_WorldRank_Item") as GameObject;
        _worldRankWindowExtended = true;
    }

    void OnEnable()
    {
        for (int i = 0; i < GridWorldRank.transform.childCount; i++)
            Destroy(GridWorldRank.transform.GetChild(i).gameObject);
        _worldRankRendered = false;    
    }

    void Update()
    {
        if (!_worldRankRendered 
            && WorldRankPersister.WorldRank.Count > 0
            && !WorldRankPersister.LoadInProgress)
        {
            CurrentPlayerPositionPanel.SetActive(false);
            int counter = 0;
            foreach (WorldRankItem worldRankItem in WorldRankPersister.WorldRank)
            {
                counter++;
                _worldRankItemObject = Instantiate(_worldRankItemPrefab);
                _worldRankItemObject.transform.SetParent(GridWorldRank.transform, false);
                _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().text = (counter).ToString() + ".";
                _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().text = worldRankItem.PlrName;
                _worldRankItemObject.transform.GetChild(2).GetComponent<Text>().text =
                    "Level: " + worldRankItem.LvlNo + "." + worldRankItem.PtsHit
                    + "\nAvg Reaction: " + worldRankItem.ReacAvg + "s";

                if (WorldRankPersister.CurrentPlayerPosition == counter)
                {
                    _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                    _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                }
                if (counter >= 100)
                    break;
            }

            ManipulatePanelSize(WorldRankPersister.CurrentPlayerPosition == 0 || WorldRankPersister.CurrentPlayerPosition <= 13);
            if (!_worldRankWindowExtended) SetCurrentPlayerValues();
            _worldRankRendered = true;
        }
    }

    void ManipulatePanelSize(bool extend)
    {
        if (_worldRankWindowExtended == extend)
            return;

        GridScrollPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(700, extend ? 1000 : 850);
        GridScrollPanel.GetComponent<RectTransform>().localPosition += new Vector3(0, extend ? -77 : 77, 0);
        CurrentPlayerPositionPanel.SetActive(!extend);
        _worldRankWindowExtended = extend;
    }

    void SetCurrentPlayerValues()
    {
        CurrentPlayerPositionPanel.SetActive(true);
        CurrentPlayerPositionPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = 
            WorldRankPersister.CurrentPlayerPosition.ToString() + ".";
        CurrentPlayerPositionPanel.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = CurrentPlayer.WorldRankItem.PlrName;
        CurrentPlayerPositionPanel.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "Level: " + CurrentPlayer.WorldRankItem.LvlNo + "." + CurrentPlayer.WorldRankItem.PtsHit
            + "\nAvg Reaction: " + CurrentPlayer.WorldRankItem.ReacAvg + "s";
    }
}
