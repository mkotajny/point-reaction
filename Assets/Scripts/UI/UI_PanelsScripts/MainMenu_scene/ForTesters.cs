using System.Collections.Generic;
using UnityEngine;

public class ForTesters : MonoBehaviour {

    void OnEnable()
    {
        //FirebasePR.InitializeFireBaseDb();
        /*
        UpdateWorldRank("1", "Marek", 2, 7, 1.5);
        UpdateWorldRank("2", "Zenek", 31, 2, 0.92);
        UpdateWorldRank("3", "Stefan", 17, 2, 0.82);
        */
        //WorldRankPersister.LoadWorldRank();
        //FirebasePR.SignOutFireBase();

        GameLevelPersister.ListAllPlayerPrefsValues();
    }

    public void ActivityLogPreview()
    {
        //ActivityLogger.PreviewLog();
        List<WorldRankItem> myWr = new List<WorldRankItem>();

        myWr =  WorldRankPersister.WorldRank;
    }

    public void UpdateWorldRank(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg)
    {
        string json;
        WorldRankItem worldRankItem = new WorldRankItem(playerId, playerName, levelNo
            , pointsHit, reactionAvg);

        json = JsonUtility.ToJson(worldRankItem);
        FirebasePR.FirebaseDbReference.Child("Top Reactors")
            .Child(playerName).SetRawJsonValueAsync(json);
    }

}
