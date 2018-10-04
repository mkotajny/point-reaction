using System;
using UnityEngine;


public class CampaignItem {

    public string Updated, PlrId, PlrName;
    public int LvlNo, HitsCmp, Lives, Ads, BnsTaken;
    public double ReacCmp;

    public CampaignItem(string playerId
        , string playerName
        , int levelNo
        , int hitsCampaign
        , int lives
        , int advertisementsWatched
        , double reactionSumCampaign
        , int bonusTaken
        )
    {
        CurrentDateToString();
        PlrId = playerId;
        PlrName = playerName;
        LvlNo = levelNo;
        HitsCmp = hitsCampaign;
        Lives = lives;
        Ads = advertisementsWatched;
        ReacCmp = reactionSumCampaign;
        BnsTaken = bonusTaken;
    }

    void CurrentDateToString()
    {
        Updated = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void ResetCampaign()
    {
        CurrentDateToString();
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

    public void SaveToFirebase(bool deleteRow = false)
    {

        if (!deleteRow)
        {
            CurrentDateToString();
            ReacCmp = Convert.ToDouble(ReacCmp.ToString("0.00"));
            Lives -= CurrentPlayer.LivesTaken;
            string json = JsonUtility.ToJson(this);
            Lives += CurrentPlayer.LivesTaken;
            FirebasePR.CampaignDbReference.Child(PlrId).SetRawJsonValueAsync(json);
            return;
        }

        FirebasePR.CampaignDbReference.Child(PlrId).SetRawJsonValueAsync(null); //delete row
    }


    public int BonusesAvailable()
    {
        return (LvlNo-1) / 5;
    }

}
