using System;


public class WorldRankItem {

    public string PlayerId, PlayerName;
    public int LevelNo, PointsHit;
    public double ReactionAvg;


    public WorldRankItem(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        LevelNo = levelNo;
        PointsHit =  pointsHit;
        ReactionAvg = reactionAvg;
    }

    public int CalculateFinalPoints()
    {
        return LevelNo*1000 
            + PointsHit*100 
            + (100-Convert.ToInt32(ReactionAvg*100));
    }
}
