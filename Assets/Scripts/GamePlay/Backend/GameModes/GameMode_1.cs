using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;


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

    public GameMode_1(GameControler gameControler, int level)
    {
        _gameControlerComponent = gameControler;
        _gameLevels = new GameLevel[30];

        for (int i = 0; i < _gameLevels.Length ; i++)
        {
            if (i==0)
                _gameLevels[i] = new GameLevel(i, _pointsStartingLivingSeconds);
            else
                _gameLevels[i] = new GameLevel(i, Mathf.Floor(_gameLevels[i-1].PointsLivingTimer.Lenght 
                    * 100 * _livingSecondsDecrease) / 100);
        }
        _currentLevel = _gameLevels[0] ;
    }

    public void ActivateSinglePoint()
    {
        if (_currentLevel.PlayStatus == LevelPlayStatuses.inProgress)
        {
            _gameControlerComponent.ActivateOneOfPoints();
            _currentLevel.BetweenPointsTimer.Deactivate();
        }
    }

    public void RegisterHit(float hitTime)
    {
        _currentLevel.RegisterHit(_hitsToWin, hitTime);
    }
}
