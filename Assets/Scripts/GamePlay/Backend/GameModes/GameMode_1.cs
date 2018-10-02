using UnityEngine;

public class GameMode_1 {

    GameControler _gameControlerComponent;
    GameLevel[] _gameLevels;
    GameLevel _currentLevel;
    int[] _livesBonuses;


    float _pointsStartingLivingSeconds = 3;
    const float _livingSecondsDecrease = .83f;

    public GameControler GameControlerComponent { get { return _gameControlerComponent; } }
    public GameLevel[] GameLevels { get { return _gameLevels; } }
    public GameLevel CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    public int[] LivesBonuses { get { return _livesBonuses; } }

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
        _currentLevel.HitsQty = 0;
        _livesBonuses = new int[] { 3, 5, 7, 10, 15, 20, 30, 50, 100};
        AdMobPR.InjectGameode(this);
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
    }

    public void SaveToFireBase(bool levelStart, UIContentManager uiContentManager = null)
    {
        CurrentPlayer.LivesTaken = (levelStart ?
            (CurrentPlayer.CampaignItem.Lives > _currentLevel.MissesToLoose ?
                _currentLevel.MissesToLoose : CurrentPlayer.CampaignItem.Lives)
            : 0);
        CurrentPlayer.CampaignItem.SaveToFirebase(_currentLevel);
        if (!levelStart && CurrentPlayer.CampaignItem.CalculateFinalPoints(_currentLevel.HitsQty) > CurrentPlayer.WorldRankItem.FinalPts)
        {
            CurrentPlayer.WorldRankItem.SaveToFirebase(_currentLevel.HitsQty);
            uiContentManager.ShowPersonalBestNotification();
        }
    }

    public void GameOver()
    {
        CurrentPlayer.CampaignsHistoryItem.SaveToFirebase();
        CurrentPlayer.CampaignItem.ResetCampaign();
        CurrentPlayer.CampaignItem.SaveToFirebase(_currentLevel, deleteRow: true);
    }
}
