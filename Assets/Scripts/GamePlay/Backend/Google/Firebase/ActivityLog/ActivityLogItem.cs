using System;
using UnityEngine;

public enum LogCategories
{
    AdLoadFailed,
    ThreeAdLoadsFailed,
    BonusWithoudAdvert,
    LevelPassed,
    LevelFailed
}

public class ActivityLogIem
{
    public string PlrId, Date, Cat, Desc;
    public int SecAftSt;

    public ActivityLogIem()
    {
        if (CurrentPlayer.CampaignItem != null) PlrId = CurrentPlayer.CampaignItem.PlrId;
        else PlrId = string.Empty;
    }

    public void Send(LogCategories logItemCategory, string logItemDescription)
    {
        if (!CheckInternet.IsConnected() || SessionVariables.TrialMode)
            return;
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        SecAftSt = (int)Time.time;
        Cat = DecodeLogCategory(logItemCategory);
        Desc = logItemDescription;

        string json = JsonUtility.ToJson(this);
        FirebasePR.ActivityLogDbReference.Child(Date.Replace(".", ":")).SetRawJsonValueAsync(json);
        return;
    }

    string DecodeLogCategory(LogCategories logItemCategory)
    {
        string logCategoryText;
        switch (logItemCategory)
        {
            case LogCategories.AdLoadFailed: logCategoryText = "Load of advert failed"; break;
            case LogCategories.ThreeAdLoadsFailed: logCategoryText = "3 failed advert loads"; break;
            case LogCategories.BonusWithoudAdvert: logCategoryText = "Bonus without advert"; break;
            case LogCategories.LevelPassed: logCategoryText = "level passed"; break;
            case LogCategories.LevelFailed: logCategoryText = "level failed"; break;
            default: logCategoryText = ""; break;
        }
        return logCategoryText;
    }
}
