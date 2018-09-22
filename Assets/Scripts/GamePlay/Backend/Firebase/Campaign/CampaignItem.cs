using System;
using UnityEngine;


public class CampaignItem {

    public string Updated, PlrId, PlrName;
    public int LvlNo;
    public int LvlMilest;
    public int HitsCmp, Lives, Ads;
    public double ReacCmp;


    public CampaignItem(string lastUpdateDate
        , string playerId
        , string playerName
        , int levelNo
        , int levelMilestoneGranted
        , int hitsCampaign
        , int lives
        , int advertisementsWatched
        , double reactionSumCampaign
        )
    {
        Updated = lastUpdateDate;
        PlrId = playerId;
        PlrName = playerName;
        LvlNo = levelNo;
        LvlMilest = levelMilestoneGranted;
        HitsCmp = hitsCampaign;
        Lives = lives;
        Ads = advertisementsWatched;
        ReacCmp = reactionSumCampaign;
    }

    public void ResetCampaign()
    {
        Updated = System.DateTime.Now.ToString("yyyy-MM-dd");
        LvlNo = 1;
        LvlMilest = 0;
        HitsCmp = 0;
        Lives = 30;
        Ads = 0;
        ReacCmp = 0;
    }

    public bool IsNewCampaign()
    {
        if (LvlNo == 1 && HitsCmp == 0 && Lives == 30)
            return true;
        return false;
    }

    public int CalculateFinalPoints(int hitsLevel)
    {
        int finalPoints;
        if (HitsCmp == 0)
            finalPoints = 0;
        else
            finalPoints = LvlNo * 10000 + hitsLevel * 1000 + (1000 - Convert.ToInt32((ReacCmp/HitsCmp) * 100));
        return finalPoints;
    }

    public void SaveToFirebase(GameLevel level)
    {
        string json = JsonUtility.ToJson(new CampaignItem(System.DateTime.Now.ToString("yyyy-MM-dd")
            , PlrId
            , PlrName
            , level.LevelNo
            , LvlMilest
            , HitsCmp
            , Lives - CurrentPlayer.LivesTaken
            , Ads
            , System.Convert.ToDouble(ReacCmp.ToString("0.00"))));

        FirebasePR.CampaignDbReference.Child(PlrId).SetRawJsonValueAsync(json);
    }

}
