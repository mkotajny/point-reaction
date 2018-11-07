using UnityEngine;

public class Timer  {
    private readonly float _length;
    private readonly bool _pointTimer;

    public bool Active { get; private set; }
    public float StartTime { get; private set; }
    public float Lenght { get { return _length; } }

    public Timer(float length, bool pointTimer)
    {
        _length = length;
        _pointTimer = pointTimer;
        Deactivate();
    }

    public void Activate()
    {
        StartTime = Time.time;
        //if (_pointTimer) _length = 3;
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
        StartTime = 0;
    }

    public bool TimeElapsed()
    {
        if (Active && Time.time > StartTime + _length)
        {
            Deactivate();
            return true;
        }
        return false;
    }

    public float TooQuick(float minimalTime)
    {
        float timeElapsed = Time.time - StartTime;
        if (Active && timeElapsed < minimalTime)
            return timeElapsed;
        return 0;
    }
}
