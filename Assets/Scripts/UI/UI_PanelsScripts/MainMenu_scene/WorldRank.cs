using UnityEngine;
using UnityEngine.UI;

public class WorldRank : MonoBehaviour {

    public GameObject GridWorldRank;
    GameObject _worldRankItemPrefab, _worldRankItemObject;

    void Awake()
    {
        _worldRankItemPrefab = Resources.Load("Panel_WorldRank_Item") as GameObject;
        //_gridWorldRank = GameObject.Find("Grid_WorldRank").GetComponent<GameObject>();

        for (int i = 0; i < 5; i++)
        {
            Instantiate(_worldRankItemPrefab).transform.SetParent(GridWorldRank.transform, false) ;
        }
    }

    void OnEnable()
    {
	}
	
}
