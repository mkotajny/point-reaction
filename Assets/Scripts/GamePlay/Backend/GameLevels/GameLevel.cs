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
    float _pointsLivingTime;

    public LevelPlayStatuses PlayStatus
    {
        get { return _playStatus; }
        set { _playStatus = value; }
    }

    public int LevelNo
    {
        get { return _levelNo; }
    }

    public float PointsLivingTime
    {
        get { return _pointsLivingTime; }
    }

    public GameLevel (int levelNo, float pointsLivingTime)
    {
        _levelNo = levelNo;
        _playStatus = LevelPlayStatuses.notStarted;
        _pointsLivingTime = pointsLivingTime;
    }
}
