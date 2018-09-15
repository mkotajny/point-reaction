using System;


public class CampaignItem {

    public string Updated, PlayerId, PlayerName;
    public int LvlNo;
    public int HitsLvl, HitsCmp, Lives, Ads;
    public double ReacCmp;


    public CampaignItem(string lastUpdateDate
        , string playerId
        , string playerName
        , int levelNo
        , int hitsLevel
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
        HitsLvl = hitsLevel;
        HitsCmp = hitsCampaign;
        Lives = lives;
        Ads = advertisementsWatched;
        ReacCmp = reactionSumCampaign;
    }
}
