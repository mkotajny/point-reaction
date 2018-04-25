using UnityEngine;

public enum LevelPlayStatuses
{
    notStarted = 0,
    inProgress = 1,
    Lost = 2,
    Win = 3
}


public class GameLevel {

    int _levelNo;
    LevelPlayStatuses _playStatus;
    int _hitsQty;
    float _reactionSum;
    float _reactionAvg;
    float _reactionFastest;

    Timer _pointsLivingTimer;
    Timer _betweenPointsTimer;


    public LevelPlayStatuses PlayStatus
    {
        get { return _playStatus; }
        set { _playStatus = value; }
    }
    public int LevelNo { get { return _levelNo; } }
    public int HitsQty { get { return _hitsQty; } }
    public float ReactionAvg { get { return _reactionAvg; } }
    public float ReactionFastest { get { return _reactionFastest; } }
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


    public GameLevel (int levelNo, float pointsLivingTime)
    {
        _levelNo = levelNo;
        _playStatus = LevelPlayStatuses.notStarted;
        _pointsLivingTimer = new Timer(pointsLivingTime);
        _betweenPointsTimer = new Timer(1);
        Restart(true);
    }

    public void RegisterHit(int hitsToWin, float hitTime)
    {
        float hitReaction = hitTime - _pointsLivingTimer.StartTime;

        _pointsLivingTimer.Deactivate();
        _hitsQty++;
        _reactionSum += hitReaction;
        _reactionAvg = _reactionSum / _hitsQty;
        if (_reactionFastest == 0 || _reactionFastest > hitReaction)
            _reactionFastest = hitReaction;

        if (_hitsQty == hitsToWin)
            _playStatus = LevelPlayStatuses.Win;
        else
            _betweenPointsTimer.Activate();
    }

    public void Restart(bool fromConstructor = false)
    {
        _hitsQty = 0;
        _reactionSum = 0;
        _reactionAvg = 0;
        _reactionFastest = 0;

        if (!fromConstructor)
        {
            _playStatus = LevelPlayStatuses.inProgress;
            _pointsLivingTimer.Deactivate();
            _betweenPointsTimer.Activate();
        }
    }
}
