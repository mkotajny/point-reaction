using UnityEngine;

public class GameMode_1
{
    private float _pointsStartingLivingSeconds = 3;
    private const float _livingSecondsDecrease = .83f;

    public GameControler GameControlerComponent { get; private set; }
    public GameLevel[] GameLevels { get; private set; }
    public GameLevel CurrentLevel { get; set; }
    public int[] LivesBonuses { get; private set; }

    public GameMode_1(GameControler gameControler)
    {
        GameControlerComponent = gameControler;
        GameLevels = new GameLevel[49];

        for (int i = 0; i < GameLevels.Length ; i++)
        {
            if (i == 0)
                GameLevels[i] = new GameLevel(i + 1, _pointsStartingLivingSeconds);
            else if (i < 10)
                GameLevels[i] = new GameLevel(i + 1, Mathf.Floor(GameLevels[i - 1].PointsLivingTimer.Lenght * _livingSecondsDecrease * 100f) / 100f);
            else
                GameLevels[i] = new GameLevel(i + 1, Mathf.Floor((GameLevels[i - 1].PointsLivingTimer.Lenght -0.01f) * 100f) / 100f);
        }
        CurrentLevel = GameLevels[CurrentPlayer.CampaignItem.LvlNo - 1];
        CurrentLevel.HitsQty = 0;
        LivesBonuses = new int[] { 3, 5, 7, 10, 15, 20, 30, 50, 100};
        AdMobPR.InjectGameode(this);
    }

    public void ActivateSinglePoint()
    {
        if (CurrentLevel.PlayStatus == LevelPlayStatuses.InProgress )
        {
            GameControlerComponent.ActivateOneOfPoints();
            CurrentLevel.BetweenPointsTimer.Deactivate();
        }
    }

    public void LevelUp()
    {
        CurrentLevel = GameLevels[CurrentLevel.LevelNo];
        CurrentPlayer.CampaignItem.LvlNo = CurrentLevel.LevelNo;
    }

    public void SaveToFireBase(bool levelStart, UIContentManager uiContentManager = null)
    {
        CurrentPlayer.LivesTaken = (levelStart ?
            (CurrentPlayer.CampaignItem.Lives > CurrentLevel.MissesToLoose ?
                CurrentLevel.MissesToLoose : CurrentPlayer.CampaignItem.Lives)
            : 0);
        CurrentPlayer.CampaignItem.SaveToFirebase();
        if (!SessionVariables.TrialMode
            && !levelStart 
            && CurrentPlayer.CampaignItem.CalculateFinalPoints(CurrentLevel.HitsQty) > CurrentPlayer.WorldRankItem.FinalPts)
        {
            CurrentPlayer.WorldRankItem.SaveToFirebase(CurrentLevel.HitsQty);
            uiContentManager.ShowPersonalBestNotification();
        }
    }

    public void GameOver()
    {
        CurrentPlayer.CampaignsHistoryItem.EndOfCampaignIntoToFirebase();
        CurrentPlayer.CampaignItem.ResetCampaign();
        CurrentPlayer.CampaignItem.SaveToFirebase(deleteRow: true);
    }
}
