using System;


public class WorldRankItem {

    public string PlayerId, PlayerName;
    public int LevelNo, PointsHit, FinalPoints;
    public double ReactionAvg;


    public WorldRankItem(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg
        )
    {
        PlayerId = playerId;
        PlayerName = playerName;
        LevelNo = levelNo;
        PointsHit =  pointsHit;
        ReactionAvg = reactionAvg;
        FinalPoints = CalculateFinalPoints();
    }

    public int CalculateFinalPoints()
    {
        return LevelNo*1000 
            + PointsHit*100 
            + (100-Convert.ToInt32(ReactionAvg*100));
    }
}
