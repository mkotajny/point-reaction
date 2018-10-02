using UnityEngine;

public class Timer  {
    bool _active;
    float _startTime;
    float _length;
    bool _pointTimer;

    public bool Active { get { return _active; } }
    public float StartTime { get { return _startTime; } }
    public float Lenght { get { return _length; } }

    public Timer(float length, bool pointTimer)
    {
        _length = length;
        _pointTimer = pointTimer;
        Deactivate();
    }

    public void Activate()
    {
        _startTime = Time.time;
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
        _startTime = 0;
    }

    public bool TimeElapsed()
    {
        if (_active && Time.time > StartTime + _length)
        {
            if (_pointTimer)
                Debug.Log("debug: point timer elapsed: length=" + _length.ToString() 
                    + "; StartTime=" + StartTime.ToString()
                    + "; CurrentTime=" + Time.time.ToString());

            Deactivate();
            return true;
        }
        return false;
    }

    public float TooQuick(float minimalTime)
    {
        float timeElapsed = Time.time - StartTime;
        if (_active && timeElapsed < minimalTime)
            return timeElapsed;
        return 0;
    }
}
