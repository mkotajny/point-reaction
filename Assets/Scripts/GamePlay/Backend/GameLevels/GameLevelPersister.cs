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

    public static void LevelSave(GameLevel currentLevel)
    {
        if (currentLevel.LevelNo < _bestLevelNoPersistence 
            || currentLevel.HitsQty == 0)
            return;

        LevelLoad();
        bool saveChanges = false;

        if (_levelPersistence.HitsQty == currentLevel.HitsQty
            && _levelPersistence.LevelNo == currentLevel.LevelNo)
        {
            if (_levelPersistence.ReactionAvg > currentLevel.ReactionAvg)
            {
                _levelPersistence.ReactionAvg = currentLevel.ReactionAvg;
                saveChanges = true;
            }
            if (_levelPersistence.ReactionFastest > currentLevel.ReactionFastest)
            {
                _levelPersistence.ReactionFastest = currentLevel.ReactionFastest;
                saveChanges = true;
            }
        }
        if (_levelPersistence.HitsQty < currentLevel.HitsQty 
            || _levelPersistence.LevelNo < currentLevel.LevelNo)
        {
            _levelPersistence.LevelNo = currentLevel.LevelNo;
            _bestLevelNoPersistence = currentLevel.LevelNo;
            _levelPersistence.HitsQty = currentLevel.HitsQty;
            _levelPersistence.ReactionAvg = currentLevel.ReactionAvg;
            _levelPersistence.ReactionFastest = currentLevel.ReactionFastest;
            saveChanges = true;
        }

        if (saveChanges)
        {
            //_levelPersistence.LevelNo = 28;
            //  _bestLevelNoPersistence = 28;
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
        PlayerPrefs.SetString("PointsHit", "0");
        PlayerPrefs.SetString("ReactionAvg", "0");
        PlayerPrefs.SetString("ReactionFastest", "0");
    }
}
