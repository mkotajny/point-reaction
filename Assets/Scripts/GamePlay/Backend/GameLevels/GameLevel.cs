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
        _pointsLivingTimer = new Timer(pointsLivingTime);
        _betweenPointsTimer = new Timer(1);
        _hitsQty = hitsQty;
    }

    public void RegisterHit(int hitsToWin, float hitTime)
    {
        float hitReaction = hitTime - _pointsLivingTimer.StartTime;
        if (hitReaction > _pointsLivingTimer.Lenght)
            hitReaction = _pointsLivingTimer.Lenght;

        _pointsLivingTimer.Deactivate();
        _hitsQty++;
        CurrentPlayer.CampaignItem.ReacCmp += hitReaction;
        CurrentPlayer.CampaignItem.HitsCmp++;

        if (_hitsQty == hitsToWin)
            PlayStatus = LevelPlayStatuses.Win;
        else
            _betweenPointsTimer.Activate();
    }

    public void Restart()
    {
        _hitsQty = 0;
        _playStatus = LevelPlayStatuses.InProgress;
        _pointsLivingTimer.Deactivate();
        _betweenPointsTimer.Activate();
    }
}
