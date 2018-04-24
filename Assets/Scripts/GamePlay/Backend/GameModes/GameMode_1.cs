using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;
    Timer _betweenPointsTimer;


    int _hitsToWin = 10;
    int _hitsQty;
    float _pointsStartingLivingSeconds = 5;
    const float _livingSecondsDecrease = .85f;

    public GameControler GameControlerComponent { get { return _gameControlerComponent; } }
    public GameLevel[] GameLevels { get { return _gameLevels; } }
    public GameLevel CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    public Timer BetweenPointsTimer
    {
        get { return _betweenPointsTimer; }
        set { _betweenPointsTimer = value; }
    }

    public GameMode_1(GameControler gameControler, int level)
    {
        _gameControlerComponent = gameControler;
        _gameLevels = new GameLevel[30];
        _betweenPointsTimer = new Timer(1);

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

    public void ActivateSinglePoint(bool startLevel = false)
    {
        if (startLevel) _hitsQty = 0;
        if (_currentLevel.PlayStatus == LevelPlayStatuses.inProgress)
        {
            _gameControlerComponent.ActivateOneOfPoints();
            _currentLevel.PointsLivingTimer.Activate();
            _betweenPointsTimer.Deactivate();
        }
    }

    public void RegisterHit()
    {
        _hitsQty++;
        if (_hitsQty == _hitsToWin)
            _currentLevel.PlayStatus = LevelPlayStatuses.Win;
        else
            _betweenPointsTimer.Activate();
    }
}
