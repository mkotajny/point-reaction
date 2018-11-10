using System;
using UnityEngine;


public class CampaignsHistoryItem
{

    public string UpdDt, PlrId, PlrName, Ver;
    public int Cmpgns, AdsWtchd, AdsSkpd;


    public CampaignsHistoryItem(string playerId
        , string playerName
        , int campaignsClosed
        , int adsWatched
        , int adsSkipped
        , string version)
    {
        CurrentDateToString();
        PlrId = playerId;
        PlrName = playerName;
        Cmpgns = campaignsClosed;
        AdsWtchd = adsWatched;
        AdsSkpd = adsSkipped;
        Ver = version;
    }

    void CurrentDateToString()
    {
        UpdDt = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void EndOfCampaignIntoToFirebase()
    {
        if (SessionVariables.TrialMode)
            return;
        CurrentDateToString();
        Cmpgns++;
        AdsWtchd += CurrentPlayer.CampaignItem.BnsTaken;
        AdsSkpd += CurrentPlayer.CampaignItem.BonusesAvailable() - CurrentPlayer.CampaignItem.BnsTaken;
        string json = JsonUtility.ToJson(this);
        FirebasePR.CampaignsHistoryDbReference.SetRawJsonValueAsync(json);
    }

    public void SaveToFirebase()
    {
        if (SessionVariables.TrialMode)
            return;
        CurrentDateToString();
        string json = JsonUtility.ToJson(this);
        FirebasePR.CampaignsHistoryDbReference.SetRawJsonValueAsync(json);
    }
}
