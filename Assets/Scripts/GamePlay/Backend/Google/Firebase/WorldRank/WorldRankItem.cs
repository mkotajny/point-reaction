using System;
using UnityEngine;

public class WorldRankItem {

    public string PlrId, PlrName;
    public int LvlNo, PtsHit, FinalPts;
    public double ReacAvg;


    public WorldRankItem(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg
        )
    {
        PlrId = playerId;
        PlrName = playerName;
        LvlNo = levelNo;
        PtsHit =  pointsHit;
        ReacAvg = reactionAvg;
        CalculateFinalPoints();
    }

    public void CalculateFinalPoints()
    {
        if (ReacAvg == 0)
            FinalPts = 0;
        else
            FinalPts = LvlNo * 10000 + PtsHit * 1000 + (1000-Convert.ToInt32(ReacAvg * 100));
    }

    public void SaveToFirebase(int hitsLevel)
    {
#if UNITY_EDITOR
        CurrentPlayer.CampaignItem.HitsCmp = 6;
        CurrentPlayer.CampaignItem.ReacCmp = 9;
#endif
        if (SessionVariables.TrialMode)
            return;

        LvlNo = CurrentPlayer.CampaignItem.LvlNo;
        if (hitsLevel != 100)
            PtsHit = hitsLevel;
        if (CurrentPlayer.CampaignItem.ReacCmp == 0)
            ReacAvg = 0;
        else
            ReacAvg = Convert.ToDouble((CurrentPlayer.CampaignItem.ReacCmp / CurrentPlayer.CampaignItem.HitsCmp).ToString("0.00"));
        CalculateFinalPoints();
        
        string json = JsonUtility.ToJson(this);
        FirebasePR.WorldRankDbReference.Child(CurrentPlayer.CampaignItem.PlrId).SetRawJsonValueAsync(json);
        WorldRankPersister.UpdateCurrentPlayer();
    }
}
