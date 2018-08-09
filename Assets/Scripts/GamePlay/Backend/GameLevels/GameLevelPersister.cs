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
            levelNo = PlayerPrefs.GetInt("LevelNo");
            bestLevelNo = PlayerPrefs.GetInt("BestLevelNo");
            pointsHit = PlayerPrefs.GetInt("PointsHit");
            reactionAvg = PlayerPrefs.GetFloat("ReactionAvg");
            reactionFastest = PlayerPrefs.GetFloat("ReactionFastest");

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
            PlayerPrefs.SetInt("LevelNo", _levelPersistence.LevelNo);
            PlayerPrefs.SetInt("BestLevelNo", _bestLevelNoPersistence);
            PlayerPrefs.SetInt("PointsHit", _levelPersistence.HitsQty);
            PlayerPrefs.SetFloat("ReactionAvg",  float.Parse(_levelPersistence.ReactionAvg.ToString("0.00")));
            PlayerPrefs.SetFloat("ReactionFastest", float.Parse(_levelPersistence.ReactionFastest.ToString("0.00")));
            PlayerPrefs.SetInt("ScoreServerUpdated", 0);
        }
    }

    public static void ResetGame()
    {
        PlayerPrefs.SetInt("LevelNo", 0);
        PlayerPrefs.SetInt("BestLevelNo", 0);
        PlayerPrefs.SetInt("PointsHit", 0);
        PlayerPrefs.SetFloat("ReactionAvg", 0);
        PlayerPrefs.SetFloat("ReactionFastest", 0);
    }

    public static void ListAllPlayerPrefsValues()
    {
        Debug.Log("debug: **** ALL PLAYERPREFS VALUES : ****");
        Debug.Log("debug: PlayerId: " + PlayerPrefs.GetString("PlayerId"));
        Debug.Log("debug: PlayerName: " + PlayerPrefs.GetString("PlayerName"));
        Debug.Log("debug: LevelNo: " + PlayerPrefs.GetInt("LevelNo"));
        Debug.Log("debug: BestLevelNo: " + PlayerPrefs.GetInt("BestLevelNo"));
        Debug.Log("debug: PointsHit: " + PlayerPrefs.GetInt("PointsHit"));
        Debug.Log("debug: ReactionAvg: " + PlayerPrefs.GetFloat("ReactionAvg"));
        Debug.Log("debug: ReactionFastest: " + PlayerPrefs.GetFloat("ReactionFastest"));
        Debug.Log("debug: ScoreServerUpdated: " + PlayerPrefs.GetInt("ScoreServerUpdated"));
        Debug.Log("debug: InGooglePlay: " + PlayerPrefs.GetInt("InGooglePlay"));
        Debug.Log("debug: **** END OF ALL PLAYERPREFS VALUES : ****");
    }
}
