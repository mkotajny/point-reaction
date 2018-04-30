using UnityEngine;

public static class GameLevelPersister  {
    static GameLevel _levelPersistence;

    public static GameLevel LevelPersistence { get { return _levelPersistence; } }

    public static void LevelLoad()
    {
        int levelNo = 0, pointsHit = 0;
        float reactionAvg = 0, reactionFastest = 0;

        try
        {
            int.TryParse(PlayerPrefs.GetString("LevelNo"), out levelNo);
            int.TryParse(PlayerPrefs.GetString("PointsHit"), out pointsHit);
            float.TryParse(PlayerPrefs.GetString("ReactionAvg"), out reactionAvg);
            float.TryParse(PlayerPrefs.GetString("ReactionFastest"), out reactionFastest);

        } catch {}

        _levelPersistence = new GameLevel(levelNo, 5, pointsHit, reactionAvg, reactionFastest);
    }

    public static void LevelSave(GameLevel levelUpdated)
    {
        LevelLoad();

        if (levelUpdated.PlayStatus == LevelPlayStatuses.Win)
        {
            _levelPersistence.LevelNo++;
            _levelPersistence.HitsQty = 0;
            _levelPersistence.ReactionAvg = 0;
            _levelPersistence.ReactionFastest = 0;
        } else
        {
            if (_levelPersistence.HitsQty < levelUpdated.HitsQty)
                _levelPersistence.HitsQty = levelUpdated.HitsQty;
            if (_levelPersistence.ReactionAvg > levelUpdated.ReactionAvg)
                _levelPersistence.ReactionAvg = levelUpdated.ReactionAvg;
            if (_levelPersistence.ReactionFastest > levelUpdated.ReactionFastest)
                _levelPersistence.ReactionFastest = levelUpdated.ReactionFastest;
        }

        PlayerPrefs.SetString("LevelNo", _levelPersistence.LevelNo.ToString());
        PlayerPrefs.SetString("PointsHit", _levelPersistence.HitsQty.ToString());
        PlayerPrefs.SetString("ReactionAvg", _levelPersistence.ReactionAvg.ToString("0.00"));
        PlayerPrefs.SetString("ReactionFastest", _levelPersistence.ReactionFastest.ToString("0.00"));
    }

    public static void ResetGame()
    {
        PlayerPrefs.SetString("LevelNo", "0");
    }
}
