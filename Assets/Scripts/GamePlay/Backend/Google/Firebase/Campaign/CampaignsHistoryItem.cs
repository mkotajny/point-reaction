using System;
using UnityEngine;


public class CampaignsHistoryItem
{

    public string UpdDt, PlrId, PlrName;
    public int Cmpgns, AdsWtchd, AdsSkpd, BnsBtnInf;


    public CampaignsHistoryItem(string playerId
        , string playerName
        , int campaignsClosed
        , int adsWatched
        , int adsSkipped
        , int howManyTimesInformedAboutBonusButton
        
        )
    {
        CurrentDateToString();
        PlrId = playerId;
        PlrName = playerName;
        Cmpgns = campaignsClosed;
        AdsWtchd = adsWatched;
        AdsSkpd = adsSkipped;
        BnsBtnInf = howManyTimesInformedAboutBonusButton;
    }

    void CurrentDateToString()
    {
        UpdDt = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void EndOfCampaignIntoToFirebase()
    {
        CurrentDateToString();
        Cmpgns++;
        AdsWtchd += CurrentPlayer.CampaignItem.BnsTaken;
        AdsSkpd += CurrentPlayer.CampaignItem.BonusesAvailable() - CurrentPlayer.CampaignItem.BnsTaken;
        string json = JsonUtility.ToJson(this);
        FirebasePR.CampaignsHistoryDbReference.Child(PlrId).SetRawJsonValueAsync(json);
    }

    public void SaveToFirebase()
    {
        CurrentDateToString();
        string json = JsonUtility.ToJson(this);
        FirebasePR.CampaignsHistoryDbReference.Child(PlrId).SetRawJsonValueAsync(json);
    }
}
