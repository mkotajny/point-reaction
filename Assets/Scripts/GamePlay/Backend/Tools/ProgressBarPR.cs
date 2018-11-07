using UnityEngine;

public enum ProgressBarPrStatuses
{
    Deactivated,
    InProgress,
    Succeded,
    Failed
}

public static class ProgressBarPR {
    static ProgressBarPrStatuses _progressStatus = ProgressBarPrStatuses.Deactivated;

    public static bool Activated { get; private set; }
    public static int ProgressStagesQty { get; private set; }
    public static float CurrentProgressValue { get; private set; }
    public static ProgressBarPrStatuses ProgressStatus { get { return _progressStatus; } }
    public static string StatusComment { get; private set; }

    public static void Activate(string statusComment, int progressStagesQty)
    {
        CurrentProgressValue = 0;
        Activated = true;
        StatusComment = statusComment;
        ProgressStagesQty = progressStagesQty;
        _progressStatus = ProgressBarPrStatuses.InProgress;
    }
    public static void Deactivate()
    {
        Activated = false;
        _progressStatus = ProgressBarPrStatuses.Deactivated;
    }
    public static void SetFail(string statusComment)
    {
        _progressStatus = ProgressBarPrStatuses.Failed;
        StatusComment = "ERROR! " + statusComment;
    }
    public static void SetSuccess()
    {
        _progressStatus = ProgressBarPrStatuses.Succeded;
    }

    public static void AddProgress(string progressComment)
    {
        if (_progressStatus != ProgressBarPrStatuses.InProgress)
            return;

        CurrentProgressValue += 1f / ProgressStagesQty;
        Debug.Log("debug: AddProgress: chk1: current progress (" + progressComment + "): " + CurrentProgressValue.ToString());

        if (CurrentProgressValue > 1)
            CurrentProgressValue = 1;
    }
}
