﻿using UnityEngine;

public class Timer  {
    bool _active;
    float _startTime;
    float _length;

    public bool Active { get { return _active; } }
    public float StartTime { get { return _startTime; } }
    public float Lenght { get { return _length; } }

    public Timer(float length)
    {
        _length = length;
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
        if (_active && Time.time > StartTime + _length)
        {
            Deactivate();
            return true;
        }
        return false;
    }
}
