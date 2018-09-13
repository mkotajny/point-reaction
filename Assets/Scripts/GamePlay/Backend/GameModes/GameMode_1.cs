using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;
    int _bestLevelNo;


    int _hitsToWin = 10;
    float _pointsStartingLivingSeconds = 3;
    const float _livingSecondsDecrease = .83f;

    public GameControler GameControlerComponent { get { return _gameControlerComponent; } }
    public GameLevel[] GameLevels { get { return _gameLevels; } }
    public GameLevel CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    public int BestLevelNo
    {
        get { return _bestLevelNo;}
        set { _bestLevelNo = value; }
    }
    public int HitsToWin { get { return _hitsToWin; } }

    public GameMode_1(GameControler gameControler)
    {
        float minus = 0.01f;
        _gameControlerComponent = gameControler;
        _gameLevels = new GameLevel[49];

        for (int i = 0; i < _gameLevels.Length ; i++)
        {
            if (i == 0)
                _gameLevels[i] = new GameLevel(i + 1, _pointsStartingLivingSeconds);
            else if (i < 10)
                _gameLevels[i] = new GameLevel(i + 1, Mathf.Floor(_gameLevels[i - 1].PointsLivingTimer.Lenght * _livingSecondsDecrease * 100f) / 100f);
            else
                _gameLevels[i] = new GameLevel(i + 1, Mathf.Floor((_gameLevels[i - 1].PointsLivingTimer.Lenght -0.01f) * 100f) / 100f);
        }
        GameLevelPersister.LevelLoad();
        _currentLevel = _gameLevels[GameLevelPersister.LevelPersistence.LevelNo];
        _currentLevel.HitsQty = GameLevelPersister.LevelPersistence.HitsQty;
        if (_currentLevel.LevelNo < GameLevelPersister.BestLevelNoPersistence)
            _currentLevel.HitsQty = 0;
        _bestLevelNo = GameLevelPersister.BestLevelNoPersistence;
    }

    public void ActivateSinglePoint()
    {
        if (_currentLevel.PlayStatus == LevelPlayStatuses.inProgress)
        {
            _gameControlerComponent.ActivateOneOfPoints();
            _currentLevel.BetweenPointsTimer.Deactivate();
        }
    }

    public void LevelUp()
    {
        _currentLevel = _gameLevels[CurrentLevel.LevelNo + 1];
        if (_currentLevel.LevelNo > _bestLevelNo)
            _bestLevelNo = _currentLevel.LevelNo;
    }
}
