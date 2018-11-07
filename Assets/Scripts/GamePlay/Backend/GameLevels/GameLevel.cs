using UnityEngine;

public enum LevelPlayStatuses
{
    notStarted = 0,
    InProgress = 1,
    Lost = 2,
    Win = 3
}


public class GameLevel
{
    public LevelPlayStatuses PlayStatus { get; set; }
    public int LevelNo { get; set; }
    public int HitsQty { get; set; }
    public int MissQty { get; set; }
    public int BonusPerfectLevel { get; private set; }
    public int HitsToWin { get; private set; }
    public int MissesToLoose { get; set; }
    public Timer PointsLivingTimer { get; set; }
    public Timer BetweenPointsTimer { get; set; }

    public GameLevel (int levelNo
        , float pointsLivingTime
        , int hitsQty = 0
        , float reactionFastest = 0)
    {
        LevelNo = levelNo;
        PlayStatus = LevelPlayStatuses.notStarted;
        PointsLivingTimer = new Timer(pointsLivingTime, true);
        BetweenPointsTimer = new Timer(2, false);
        HitsQty = hitsQty;
        MissQty = 0;
        BonusPerfectLevel = 1;
        if (levelNo >= 11) BonusPerfectLevel = 2;
        if (levelNo >= 16) BonusPerfectLevel = 3;
        if (levelNo >= 21) BonusPerfectLevel = 4;
        if (levelNo >= 26) BonusPerfectLevel = 5;
        if (levelNo >= 31) BonusPerfectLevel = 10;
        if (levelNo >= 36) BonusPerfectLevel = 15;
        if (levelNo >= 41) BonusPerfectLevel = 20;
        if (levelNo >= 1) HitsToWin = 2;
        if (levelNo >= 2) HitsToWin = 3;
        if (levelNo >= 4) HitsToWin = 4;
        if (levelNo >= 7) HitsToWin = 5;
        if (levelNo >= 11) HitsToWin = 6;
        if (levelNo >= 16) HitsToWin = 8;
        if (levelNo >= 22) HitsToWin = 10;
        if (levelNo >= 30) HitsToWin = 12;
        if (levelNo >= 40) HitsToWin = 15;
        if (levelNo >= 1) MissesToLoose = 2;
        if (levelNo >= 11) MissesToLoose = 3;
        if (levelNo >= 21) MissesToLoose = 4;
        if (levelNo >= 26) MissesToLoose = 5;
        if (levelNo >= 31) MissesToLoose = 10;
        if (levelNo >= 36) MissesToLoose = 15;
        if (levelNo >= 41) MissesToLoose = 20;
    }

    float CalculateShotReaction(float hitTime)
    {
        float hitReaction = hitTime - PointsLivingTimer.StartTime;
        if (hitReaction > PointsLivingTimer.Lenght)
            hitReaction = PointsLivingTimer.Lenght;
        return hitReaction;
    }


    public void RegisterHit(int hitsToWin, float hitTime)
    {
        float hitReaction = CalculateShotReaction(hitTime);

        PointsLivingTimer.Deactivate();
        HitsQty++;
        CurrentPlayer.CampaignItem.ReacCmp += hitReaction;
        CurrentPlayer.CampaignItem.HitsCmp++;

        if (HitsQty == hitsToWin)
            PlayStatus = LevelPlayStatuses.Win;
        else
            BetweenPointsTimer.Activate();
    }

    public void RegisterFail(ScreenTouchTypes touchType)
    {
        if (touchType == ScreenTouchTypes.Miss)
        {
            float shotReaction = CalculateShotReaction(Time.time);
            CurrentPlayer.CampaignItem.ReacCmp += shotReaction;
            CurrentPlayer.CampaignItem.HitsCmp++;
        }
        MissQty++;
        CurrentPlayer.CampaignItem.Lives--;
    }
    public void Reset()
    {
        HitsQty = 0;
        MissQty = 0;
        MissesToLoose = (MissesToLoose < CurrentPlayer.CampaignItem.Lives) ? MissesToLoose : CurrentPlayer.CampaignItem.Lives;
    }

    public void SpawnPoint()
    {
        PointsLivingTimer.Deactivate();
        BetweenPointsTimer.Activate();
    }
}
