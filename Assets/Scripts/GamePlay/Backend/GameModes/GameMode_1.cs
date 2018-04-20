using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currenLevel;


    int _hitsToWin = 10;
    int _hitsQty;
    float _pointsLivingSeconds = 5;
    const float _livingSecondsDecrease = .85f;

    public GameControler GameControlerComponent
    {
        get { return _gameControlerComponent; }
    }
    public GameLevel[] GameLevels
    {
        get { return _gameLevels; }
    }
    public GameLevel CurrentLevel
    {
        get { return _currenLevel; }
        set { _currenLevel = value; }
    }


    public GameMode_1(GameControler gameControler, int level)
    {
        _gameControlerComponent = gameControler;
        _gameLevels = new GameLevel[30];

        for (int i = 0; i < _gameLevels.Length ; i++)
        {
            _gameLevels[i] = new GameLevel(i, _pointsLivingSeconds);
            _pointsLivingSeconds = Mathf.Floor(_pointsLivingSeconds * 100 * _livingSecondsDecrease) / 100;
        }
        _currenLevel = _gameLevels[0] ;
    }

    public void ActivateSinglePoint(bool startLevel = false)
    {
        if (startLevel)
            _hitsQty = 0;

        _gameControlerComponent.ActivateOneOfPoints();
    }

    public void RegisterHit()
    {
        _hitsQty++;
        if (_hitsQty == _hitsToWin)
            _currenLevel.PlayStatus = LevelPlayStatuses.Win;
    }
}
