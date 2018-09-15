﻿using UnityEngine;
using Firebase.Database;

public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;


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
    public int HitsToWin { get { return _hitsToWin; } }

    public GameMode_1(GameControler gameControler)
    {
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
        _currentLevel = _gameLevels[CurrentPlayer.CampaignItem.LvlNo - 1];
        Debug.Log("debug: GameMode_1: " + _currentLevel.LevelNo.ToString() );
        _currentLevel.HitsQty = 0;
    }

    public void ActivateSinglePoint()
    {
        if (_currentLevel.PlayStatus == LevelPlayStatuses.InProgress )
        {
            _gameControlerComponent.ActivateOneOfPoints();
            _currentLevel.BetweenPointsTimer.Deactivate();
        }
    }

    public void LevelUp()
    {
        _currentLevel = _gameLevels[CurrentLevel.LevelNo];
        CurrentPlayer.CampaignItem.LvlNo = _currentLevel.LevelNo;
        CurrentPlayer.CampaignItem.HitsLvl = 0;
        SaveToFireBase(levelWin: true);
    }

    public void SaveToFireBase(bool levelWin)
    {
        string json = JsonUtility.ToJson(new CampaignItem(System.DateTime.Now.ToString("yyyy-MM-dd")
            , CurrentPlayer.CampaignItem.PlayerId
            , CurrentPlayer.CampaignItem.PlayerName
            , _currentLevel.LevelNo
            , levelWin ? 0 : _currentLevel.HitsQty
            , CurrentPlayer.CampaignItem.HitsCmp
            , CurrentPlayer.CampaignItem.Lives
            , CurrentPlayer.CampaignItem.Ads
            , System.Convert.ToDouble(CurrentPlayer.CampaignItem.ReacCmp.ToString("0.00"))));

        FirebasePR.CampaignDbReference.Child(CurrentPlayer.PlayerId).SetRawJsonValueAsync(json);
    }
}
