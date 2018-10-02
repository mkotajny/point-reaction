﻿using System;
using UnityEngine;


public class CampaignsHistoryItem
{

    public string UpdDt, PlrId, PlrName;
    public int Cmpgns, AdsWtchd, AdsSkpd;


    public CampaignsHistoryItem(string playerId
        , string playerName
        , int campaignsClosed
        , int adsWatched
        , int adsSkipped)
    {
        CurrentDateToString();
        PlrId = playerId;
        PlrName = playerName;
        Cmpgns = campaignsClosed;
        AdsWtchd = adsWatched;
        AdsSkpd = adsSkipped;
    }

    void CurrentDateToString()
    {
        UpdDt = DateTime.Now.ToString("yyyy-MM-dd");
    }

    public void SaveToFirebase()
    {
        CurrentDateToString();
        Cmpgns++;
        AdsWtchd += CurrentPlayer.CampaignItem.BnsTaken;
        AdsSkpd += CurrentPlayer.CampaignItem.BonusesAvailable() - CurrentPlayer.CampaignItem.BnsTaken;
        string json = JsonUtility.ToJson(this);
        FirebasePR.CampaignsHistoryDbReference.Child(PlrId).SetRawJsonValueAsync(json);
    }
}
