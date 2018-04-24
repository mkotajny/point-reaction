using UnityEngine;

public class Timer  {
    float _startTime;
    bool _active;
    float _lenght;

    public bool Active { get { return _active; } }
    public float Lenght { get { return _lenght; } }

    public Timer(float length)
    {
        _lenght = length;
        Deactivate();
    }

    public void Activate()
    {
        _active = true;
        _startTime = Time.time;
    }

    public void Deactivate()
    {
        _active = false;
        _startTime = 0;
    }

    public bool TimeElapsed()
    {
        if (_active && Time.time > _startTime + _lenght)
        {
            Deactivate();
            return true;
        }
        return false;
    }
}
