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
    Timer _pointsLivingTimer;


    public LevelPlayStatuses PlayStatus
    {
        get { return _playStatus; }
        set { _playStatus = value; }
    }

    public int LevelNo
    {
        get { return _levelNo; }
    }

    public Timer PointsLivingTimer
    {
        get { return _pointsLivingTimer; }
        set { _pointsLivingTimer = value; }
    }

    public GameLevel (int levelNo, float pointsLivingTime)
    {
        _levelNo = levelNo;
        _playStatus = LevelPlayStatuses.notStarted;
        _pointsLivingTimer = new Timer(pointsLivingTime);
    }
}
