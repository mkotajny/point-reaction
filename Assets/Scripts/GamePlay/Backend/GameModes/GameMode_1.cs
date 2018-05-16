using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;
    int _bestLevelNo;


    int _hitsToWin = 10;
    float _pointsStartingLivingSeconds = 5;
    const float _livingSecondsDecrease = .85f;

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
        _gameControlerComponent = gameControler;
        _gameLevels = new GameLevel[31];

        for (int i = 0; i < _gameLevels.Length ; i++)
        {
            if (i==0)
                _gameLevels[i] = new GameLevel(i, _pointsStartingLivingSeconds);
            else
                _gameLevels[i] = new GameLevel(i, Mathf.Floor(_gameLevels[i-1].PointsLivingTimer.Lenght 
                    * 100 * _livingSecondsDecrease) / 100);
        }
        GameLevelPersister.LevelLoad();
        _currentLevel = _gameLevels[GameLevelPersister.LevelPersistence.LevelNo];
        _currentLevel.HitsQty = GameLevelPersister.LevelPersistence.HitsQty;
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
