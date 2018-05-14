using UnityEngine;

public static class GameLevelPersister  {
    static GameLevel _levelPersistence;
    static int _bestLevelNoPersistence;

    public static GameLevel LevelPersistence { get { return _levelPersistence; } }
    public static int BestLevelNoPersistence { get { return _bestLevelNoPersistence; } }

    public static void LevelLoad()
    {
        int levelNo = 0, bestLevelNo = 0, pointsHit = 0;
        float reactionAvg = 0, reactionFastest = 0;

        try
        {
            int.TryParse(PlayerPrefs.GetString("LevelNo"), out levelNo);
            int.TryParse(PlayerPrefs.GetString("BestLevelNo"), out bestLevelNo);
            int.TryParse(PlayerPrefs.GetString("PointsHit"), out pointsHit);
            float.TryParse(PlayerPrefs.GetString("ReactionAvg"), out reactionAvg);
            float.TryParse(PlayerPrefs.GetString("ReactionFastest"), out reactionFastest);

        } catch {}

        _levelPersistence = new GameLevel(levelNo, 5, pointsHit, reactionAvg, reactionFastest);
        _bestLevelNoPersistence = bestLevelNo;
    }

    public static void LevelSave(GameLevel levelUpdated)
    {
        if (_levelPersistence.LevelNo < _bestLevelNoPersistence)
            return;

        LevelLoad();
        bool saveChanges = false;

        if (levelUpdated.PlayStatus == LevelPlayStatuses.Win)
        {
            _levelPersistence.LevelNo++;
            _levelPersistence.HitsQty = 0;
            _levelPersistence.ReactionAvg = 0;
            _levelPersistence.ReactionFastest = 0;
            if (_levelPersistence.LevelNo > _bestLevelNoPersistence)
                _bestLevelNoPersistence++;

            saveChanges = true;
        } else
        {
            if (_levelPersistence.HitsQty == levelUpdated.HitsQty)
            {
                if (_levelPersistence.ReactionAvg > levelUpdated.ReactionAvg)
                {
                    _levelPersistence.ReactionAvg = levelUpdated.ReactionAvg;
                    saveChanges = true;
                }
                if (_levelPersistence.ReactionFastest > levelUpdated.ReactionFastest)
                {
                    _levelPersistence.ReactionFastest = levelUpdated.ReactionFastest;
                    saveChanges = true;
                }
            }
            if (_levelPersistence.HitsQty < levelUpdated.HitsQty)
            {
                _levelPersistence.HitsQty = levelUpdated.HitsQty;
                _levelPersistence.ReactionAvg = levelUpdated.ReactionAvg;
                _levelPersistence.ReactionFastest = levelUpdated.ReactionFastest;
                saveChanges = true;
            }
        }

        if (saveChanges)
        {
            PlayerPrefs.SetString("LevelNo", _levelPersistence.LevelNo.ToString());
            PlayerPrefs.SetString("BestLevelNo", _bestLevelNoPersistence.ToString());
            PlayerPrefs.SetString("PointsHit", _levelPersistence.HitsQty.ToString());
            PlayerPrefs.SetString("ReactionAvg", _levelPersistence.ReactionAvg.ToString("0.00"));
            PlayerPrefs.SetString("ReactionFastest", _levelPersistence.ReactionFastest.ToString("0.00"));
        }
    }

    public static void ResetGame()
    {
        PlayerPrefs.SetString("LevelNo", "0");
        PlayerPrefs.SetString("BestLevelNo", "0");
    }
}
