using System;


public class CampaignItem {

    public string Updated, PlayerId, PlayerName;
    public int LvlNo;
    public int LvlMilest;
    public int HitsCmp, Lives, Ads;
    public double ReacCmp;


    public CampaignItem(string lastUpdateDate
        , string playerId
        , string playerName
        , int levelNo
        , int levelMilesToneGranted
        , int hitsCampaign
        , int lives
        , int advertisementsWatched
        , double reactionSumCampaign
        )
    {
        Updated = lastUpdateDate;
        PlayerId = playerId;
        PlayerName = playerName;
        LvlNo = levelNo;
        LvlMilest = levelMilesToneGranted;
        HitsCmp = hitsCampaign;
        Lives = lives;
        Ads = advertisementsWatched;
        ReacCmp = reactionSumCampaign;
    }
}
