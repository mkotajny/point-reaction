using System;
using System.IO;
using UnityEngine;

public static class ActivityLogger {
    static int _logSize;
    static string[] _activityLog;
    static int _lastIndex;
    static string _filePath;

    public static void InitializeLog()
    {
        _logSize = 1000;
        if (_activityLog == null)
            _activityLog = new string[0];
        _filePath = Application.persistentDataPath + "/ActivityLog.txt";
        
        if (File.Exists(_filePath))
        {
            _activityLog = File.ReadAllLines(_filePath);
            _lastIndex = File.ReadAllLines(_filePath).Length;
        } else _lastIndex = 0;
    }

    public static void AddLogLine(string line)
    {
        Array.Resize<string>(ref _activityLog, _lastIndex + 1);
        _activityLog[_lastIndex] = DateTime.Now.ToString("s").Replace("T"," ") + ": " + line;
        _lastIndex++;
    }

    public static void SaveLog()
    {
        int startingIndex;

        if (_lastIndex <= _logSize)
            startingIndex = 0;
        else
            startingIndex = _lastIndex - _logSize;

        var sr = File.CreateText(_filePath);
        for (int i = startingIndex; i < _lastIndex; i++)
            sr.WriteLine(_activityLog[i]);
        sr.Close();
    }

    public static void PreviewLog()
    {
        Application.OpenURL(_filePath);
    }
}
