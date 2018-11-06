using System;
using UnityEngine;

public enum LogCategories
{
    LevelPassed,
    LevelFailed
}

public class ActivityLogIem
{
    public string PlrId, Date, Cat, Desc;
    public int SecAftSt;

    public ActivityLogIem()
    {
        PlrId = CurrentPlayer.CampaignItem.PlrId;
    }

    public void Add(LogCategories logItemCategory, string logItemDescription)
    {
        if (!CheckInternet.IsConnected())
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
            case LogCategories.LevelFailed: logCategoryText = "Level failed"; break;
            case LogCategories.LevelPassed: logCategoryText = "Level passed"; break;
            default: logCategoryText = ""; break;
        }
        return logCategoryText;
    }
}
