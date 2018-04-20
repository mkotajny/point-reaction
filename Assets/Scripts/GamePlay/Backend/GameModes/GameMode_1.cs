using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel _gameLevel;
    int _hitsToWin = 10;

    int _hitsQty;

    public GameControler GameControlerComponent
    {
        get { return _gameControlerComponent; }
    }

    public GameLevel GameLevel
    {
        get { return _gameLevel; }
    }

    public GameMode_1(GameControler gameControler, int level)
    {
        _gameControlerComponent = gameControler;
        _gameLevel = new GameLevel(0);
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
            _gameLevel.PlayStatus = LevelPlayStatuses.Win;
    }
}
