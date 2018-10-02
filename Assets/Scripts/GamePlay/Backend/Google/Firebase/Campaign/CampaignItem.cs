using System;
using UnityEngine;


public class CampaignItem {

    public string Updated, PlrId, PlrName;
    public int LvlNo;
    public int HitsCmp, Lives, Ads;
    public double ReacCmp;
    public int BnsTaken;



    public CampaignItem(string lastUpdateDate
        , string playerId
        , string playerName
        , int levelNo
        , int hitsCampaign
        , int lives
        , int advertisementsWatched
        , double reactionSumCampaign
        , int bonusTaken
        )
    {
        Updated = lastUpdateDate;
        PlrId = playerId;
        PlrName = playerName;
        LvlNo = levelNo;
        HitsCmp = hitsCampaign;
        Lives = lives;
        Ads = advertisementsWatched;
        ReacCmp = reactionSumCampaign;
        BnsTaken = bonusTaken;
    }

    public void ResetCampaign()
    {
        Updated = System.DateTime.Now.ToString("yyyy-MM-dd");
        LvlNo = 1;
        HitsCmp = 0;
        Lives = 30;
        Ads = 0;
        ReacCmp = 0;
        BnsTaken = 0;
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
            , HitsCmp
            , Lives - CurrentPlayer.LivesTaken
            , Ads
            , Convert.ToDouble(ReacCmp.ToString("0.00"))
            , BnsTaken));

        FirebasePR.CampaignDbReference.Child(PlrId).SetRawJsonValueAsync(json);
    }

    public int BonusesAvailable()
    {
        return (LvlNo-1) / 5;
    }

}
