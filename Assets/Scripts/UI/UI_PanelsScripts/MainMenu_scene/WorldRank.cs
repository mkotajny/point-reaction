using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;

public class WorldRank : MonoBehaviour {

    public GameObject GridWorldRank, PanelWorldRank, PanelMainMenu;
    GameObject _worldRankItemPrefab, _worldRankItemObject;
    bool _worldRankLoaded;
    string _currentPlayerNameText;
    
    void Awake()
    {
        _worldRankItemPrefab = Resources.Load("Panel_WorldRank_Item") as GameObject;
    }

    void OnEnable()
    {
        _currentPlayerNameText = GameObject.Find("PlayerName_background").GetComponent<Text>().text;
        for (int i = 0; i < GridWorldRank.transform.childCount; i++)
            Destroy(GridWorldRank.transform.GetChild(i).gameObject);

        _worldRankLoaded = false;

        if (!CheckInternet.IsConnected())
        {
            new Alert("No internet !", "Please connect with the internet and try again.")
                .SetPositiveButton("OK", () => { }).Show();
            PanelMainMenu.SetActive(true);
            PanelWorldRank.SetActive(false);
        }
        else
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

                if (worldRankItem.PlayerName == _currentPlayerNameText)
                {
                    _worldRankItemObject.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                    _worldRankItemObject.transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                    int playersPointsFromDisk = PlayerPrefs.GetInt("BestLevelNo") * 1000
                        + PlayerPrefs.GetInt("PointsHit") * 100
                        + (100 - System.Convert.ToInt32((PlayerPrefs.GetFloat("ReactionAvg") * 100)));

                    if (playersPointsFromDisk > worldRankItem.CalculateFinalPoints())
                    {
                        _worldRankItemObject.transform.GetChild(2).GetComponent<Text>().text =
                            "Level: " + PlayerPrefs.GetInt("BestLevelNo").ToString() + "." 
                            + PlayerPrefs.GetInt("PointsHit").ToString()
                            + "\nAvg Reaction: " + PlayerPrefs.GetFloat("ReactionAvg").ToString() + "s";
                    }
                }

                counter++;
            }
            _worldRankLoaded = true;
        }
    }
}
