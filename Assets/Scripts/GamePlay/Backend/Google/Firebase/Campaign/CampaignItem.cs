using System;
using UnityEngine;
using PaperPlaneTools;

public class CampaignItem {

    public string Updated, PlrId, PlrName;
    public int LvlNo, HitsCmp, Lives, Ads, BnsTaken, BnsLastMlstn;
    public double ReacCmp;

    public CampaignItem(string playerId
        , string playerName
        , int levelNo
        , int hitsCampaign
        , int lives
        , int advertisementsWatched
        , double reactionSumCampaign
        , int bonusTaken
        , int bonusLastMilestone
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
        BnsLastMlstn = bonusLastMilestone;
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
        Lives = 10;
        Ads = 0;
        ReacCmp = 0;
        BnsTaken = 0;
        BnsLastMlstn = 0;
    }

    public bool IsNewCampaign()
    {
        if (LvlNo == 1 && HitsCmp == 0 && Lives == 10)
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
        if (!CheckInternet.IsConnected())
        {
            new Alert("No internet !", "Please connect with the internet and run the game again.")
                .SetPositiveButton("OK", () => 
                {
                    SessionVariables.CurrentScene = SessionVariables.PRScenes.Quit;
                    Initiate.Fade("BlackScene", Color.black, 1.0f);
                }).Show();

            return;
        }

        if (CurrentPlayer.TrialMode)
            return;

        if (!deleteRow)
        {
            CurrentDateToString();
            ReacCmp = Convert.ToDouble(ReacCmp.ToString("0.00"));
            Lives -= CurrentPlayer.LivesTaken;
            string json = JsonUtility.ToJson(this);
            Lives += CurrentPlayer.LivesTaken;
            /*Debug.Log("debug: SaveToFirebase: chk1: firebase credentials: "
                + FirebasePR.FirebaseAuth.CurrentUser.UserId + "/"
                + FirebasePR.FirebaseAuth.CurrentUser.DisplayName);*/

            FirebasePR.CampaignDbReference.SetRawJsonValueAsync(json);
            return;
        }

        FirebasePR.CampaignDbReference.SetRawJsonValueAsync(null); //delete row
    }


    public int BonusesAvailable()
    {
        return (LvlNo-1) / 5;
    }

    public bool BonusTakenInCurrentMilestone()
    {
        return (LvlNo - 1) / 5 == BnsLastMlstn;
    }

}
