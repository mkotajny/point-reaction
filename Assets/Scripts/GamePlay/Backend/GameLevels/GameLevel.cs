using UnityEngine;

public enum LevelPlayStatuses
{
    notStarted = 0,
    InProgress = 1,
    Lost = 2,
    Win = 3
}


public class GameLevel {

    int _levelNo;
    LevelPlayStatuses _playStatus;
    int _hitsQty;
    int _missQty;
    int _bonusPerfectLevel;
    int _hitsToWin;
    int _missesToLoose;
    Timer _pointsLivingTimer;
    Timer _betweenPointsTimer;

    public LevelPlayStatuses PlayStatus
    {
        get { return _playStatus; }
        set
        {
            _playStatus = value;
        }
    }
    public int LevelNo
    {
        get { return _levelNo; }
        set { _levelNo = value; }
    }
    public int HitsQty
    {
        get { return _hitsQty; }
        set { _hitsQty = value; }
    }
    public int MissQty
    {
        get { return _missQty; }
        set { _missQty = value; }
    }
    public int BonusPerfectLevel { get { return _bonusPerfectLevel; } }
    public int HitsToWin { get { return _hitsToWin; } }
    public int MissesToLoose { get { return _missesToLoose; } }
    public Timer PointsLivingTimer
    {
        get { return _pointsLivingTimer; }
        set { _pointsLivingTimer = value; }
    }
    public Timer BetweenPointsTimer
    {
        get { return _betweenPointsTimer; }
        set { _betweenPointsTimer = value; }
    }

    public GameLevel (int levelNo
        , float pointsLivingTime
        , int hitsQty = 0
        , float reactionFastest = 0)
    {
        _levelNo = levelNo;
        _playStatus = LevelPlayStatuses.notStarted;
        _pointsLivingTimer = new Timer(pointsLivingTime, true);
        _betweenPointsTimer = new Timer(2, false);
        _hitsQty = hitsQty;
        _missQty = 0;
        _bonusPerfectLevel = 1;
        if (levelNo >= 11) _bonusPerfectLevel = 2;
        if (levelNo >= 16) _bonusPerfectLevel = 3;
        if (levelNo >= 21) _bonusPerfectLevel = 4;
        if (levelNo >= 26) _bonusPerfectLevel = 5;
        if (levelNo >= 31) _bonusPerfectLevel = 10;
        if (levelNo >= 36) _bonusPerfectLevel = 15;
        if (levelNo >= 41) _bonusPerfectLevel = 20;
        if (levelNo >= 1) _hitsToWin = 2;
        if (levelNo >= 2) _hitsToWin = 3;
        if (levelNo >= 4) _hitsToWin = 4;
        if (levelNo >= 7) _hitsToWin = 5;
        if (levelNo >= 11) _hitsToWin = 6;
        if (levelNo >= 16) _hitsToWin = 8;
        if (levelNo >= 22) _hitsToWin = 10;
        if (levelNo >= 30) _hitsToWin = 12;
        if (levelNo >= 40) _hitsToWin = 15;
        if (levelNo >= 1) _missesToLoose = 2;
        if (levelNo >= 11) _missesToLoose = 3;
        if (levelNo >= 21) _missesToLoose = 4;
        if (levelNo >= 26) _missesToLoose = 5;
        if (levelNo >= 31) _missesToLoose = 10;
        if (levelNo >= 36) _missesToLoose = 15;
        if (levelNo >= 41) _missesToLoose = 20;
    }

    float CalculateShotReaction(float hitTime)
    {
        float hitReaction = hitTime - _pointsLivingTimer.StartTime;
        if (hitReaction > _pointsLivingTimer.Lenght)
            hitReaction = _pointsLivingTimer.Lenght;
        return hitReaction;
    }


    public void RegisterHit(int hitsToWin, float hitTime)
    {
        float hitReaction = CalculateShotReaction(hitTime);

        _pointsLivingTimer.Deactivate();
        _hitsQty++;
        CurrentPlayer.CampaignItem.ReacCmp += hitReaction;
        CurrentPlayer.CampaignItem.HitsCmp++;

        if (_hitsQty == hitsToWin)
            PlayStatus = LevelPlayStatuses.Win;
        else
            _betweenPointsTimer.Activate();
    }

    public void RegisterFail(ScreenTouchTypes touchType)
    {
        if (touchType == ScreenTouchTypes.Miss)
        {
            float shotReaction = CalculateShotReaction(Time.time);
            CurrentPlayer.CampaignItem.ReacCmp += shotReaction;
            CurrentPlayer.CampaignItem.HitsCmp++;
        }
        _missQty++;
        CurrentPlayer.CampaignItem.Lives--;
    }
    public void Reset()
    {
        _hitsQty = 0;
        _missQty = 0;
    }

    public void SpawnPoint()
    {
        _pointsLivingTimer.Deactivate();
        _betweenPointsTimer.Activate();
    }
}
