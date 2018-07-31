using System;


public class WorldRankItem {

    public string PlayerId, PlayerName;
    public int LevelNo, PointsHit;
    public double ReactionAvg, ReactionFastest;


    public WorldRankItem(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg
        , double reactionFastest)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        LevelNo = levelNo;
        PointsHit =  pointsHit;
        ReactionAvg = reactionAvg;
        ReactionFastest = reactionFastest;
    }
}
