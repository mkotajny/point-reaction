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

    public LevelPlayStatuses PlayStatus
    {
        get { return _playStatus; }
        set { _playStatus = value; }
    }

    public int LevelNo
    {
        get { return _levelNo; }
        set { _levelNo = value; }
    }

    public GameLevel (int levelNo)
    {
        _levelNo = levelNo;
        _playStatus = LevelPlayStatuses.notStarted;
    }
}
