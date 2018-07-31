using System.Collections.Generic;
using UnityEngine;

public class ForTesters : MonoBehaviour {

    void OnEnable()
    {
        //ActivityLogger.AddLogLine("FOR TESTERS panel has been opened");

        //UpdateWorldRank("1", "Marek", 2, 7, 1.5, 1.1);
        //UpdateWorldRank("2", "Zenek", 31, 2, 0.92, 1.12);
        WorldRankPersister.LoadWorldRank();
    }

    public void ActivityLogPreview()
    {
        //ActivityLogger.AddLogLine("Activity log has been opened");
        //ActivityLogger.PreviewLog();
        List<WorldRankItem> myWr = new List<WorldRankItem>();
        string testVal;

        myWr =  WorldRankPersister.WorldRank;
        testVal = "aaa";
    }

    public void UpdateWorldRank(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg
        , double reactionFastest)
    {
        string json;
        WorldRankItem worldRankItem = new WorldRankItem(playerId, playerName, levelNo
            , pointsHit, reactionAvg, reactionFastest);

        json = JsonUtility.ToJson(worldRankItem);
        FirebasePR.FirebaseDbReference.Child("Top Reactors")
            .Child(playerName).SetRawJsonValueAsync(json);
    }

}
