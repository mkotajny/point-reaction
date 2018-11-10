
using Firebase.Database;
using UnityEngine;

public static class SessionVariables  {
    public enum PRScenes
    {
        GameLoading,
        MainMenu,
        GameBoard,
        Quit
    }

    static CampaignItem _campaignItemCopy;

    public static PRScenes CurrentScene { get; set; }
    public static ActivityLogIem ActivityLog { get; set; }
    public static bool TrialMode { get; set; }
    public static GameVersion CurrentGameVersion { get; private set; }
    public static GameVersion LatestCriticalUpdateVersion { get; private set; }

    public static void SetSession()
    {
        ActivityLog = new ActivityLogIem();
        CurrentGameVersion = new GameVersion(Application.version);

        #if UNITY_EDITOR
            SetSessionForEditor();
        #endif
    }

    static void SetSessionForEditor()
    {
        if (CurrentPlayer.CampaignItem == null)
        {
            CurrentPlayer.CampaignItem = new CampaignItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 1, 0, 10, 0, 0, 0, CurrentGameVersion.VersionString);
            CurrentPlayer.CampaignsHistoryItem = new CampaignsHistoryItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 0, 0, 0, CurrentGameVersion.VersionString);
            CurrentPlayer.WorldRankItem = new WorldRankItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 0, 6, 0.62);
            TrialMode = true;
        }
        MusicPR.SetVolumeSfx(1f);
        MusicPR.SetVolumeMusic(0.5f);
    }

    public static void SetTrialMode()
    {

        if (CurrentPlayer.CampaignItem != null)
            _campaignItemCopy = new CampaignItem(CurrentPlayer.CampaignItem.PlrId, CurrentPlayer.CampaignItem.PlrName
                , CurrentPlayer.CampaignItem.LvlNo, CurrentPlayer.CampaignItem.HitsCmp, CurrentPlayer.CampaignItem.Lives
                , CurrentPlayer.CampaignItem.ReacCmp, CurrentPlayer.CampaignItem.BnsTaken, CurrentPlayer.CampaignItem.BnsLastMlstn
                , CurrentGameVersion.VersionString);
        CurrentPlayer.CampaignItem = new CampaignItem(string.Empty, string.Empty, 1, 0, 10, 0, 0, 0, SessionVariables.CurrentGameVersion.VersionString);
        TrialMode = true;
    }
    public static void EndTrialMode()
    {
        if (_campaignItemCopy != null)
            CurrentPlayer.CampaignItem = new CampaignItem(_campaignItemCopy.PlrId, _campaignItemCopy.PlrName, _campaignItemCopy.LvlNo, _campaignItemCopy.HitsCmp
            , _campaignItemCopy.Lives, _campaignItemCopy.ReacCmp, _campaignItemCopy.BnsTaken, _campaignItemCopy.BnsLastMlstn
            , CurrentGameVersion.VersionString);
        TrialMode = false;
    }

    public static void GetGameSettingsData()
    {
        FirebasePR.GameSettingsReference
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null)
                        LatestCriticalUpdateVersion = new GameVersion(snapshot.Child("LastCriticalUpdateVersion").Value.ToString());
                    ProgressBarPR.AddProgress("get game settings data");
                }
                return;
            });
    }
}
