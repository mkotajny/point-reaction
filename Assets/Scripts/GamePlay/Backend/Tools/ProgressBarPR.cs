using UnityEngine;

public enum ProgressBarPrStatuses
{
    Deactivated,
    InProgress,
    Succeded,
    Failed
}

public static class ProgressBarPR {

    static bool _activated;
    static int _progressStagesQty;
    static float _currentProgressValue;
    static ProgressBarPrStatuses _progressStatus = ProgressBarPrStatuses.Deactivated;
    static string _statusComment;

    public static bool Activated { get { return _activated; } }
    public static int ProgressStagesQty { get { return _progressStagesQty; } }
    public static float CurrentProgressValue { get { return _currentProgressValue; } }
    public static ProgressBarPrStatuses ProgressStatus { get { return _progressStatus; } }
    public static string StatusComment { get { return _statusComment; } }

    public static void Activate(string statusComment, int progressStagesQty)
    {
        _currentProgressValue = 0;
        _activated = true;
        _statusComment = statusComment;
        _progressStagesQty = progressStagesQty;
        _progressStatus = ProgressBarPrStatuses.InProgress;
    }
    public static void Deactivate()
    {
        _activated = false;
        _progressStatus = ProgressBarPrStatuses.Deactivated;
    }
    public static void SetFail(string statusComment)
    {
        _progressStatus = ProgressBarPrStatuses.Failed;
        _statusComment = "ERROR! " + statusComment;
    }
    public static void SetSuccess()
    {
        _progressStatus = ProgressBarPrStatuses.Succeded;
    }

    public static void AddProgress(string progressComment)
    {
        if (_progressStatus != ProgressBarPrStatuses.InProgress)
            return;

        _currentProgressValue += 1f / _progressStagesQty;
        //Debug.Log("debug: AddProgress: chk1: current progress (" + progressComment + "): " + _currentProgressValue.ToString());

        if (_currentProgressValue > 1)
            _currentProgressValue = 1;
    }
}
