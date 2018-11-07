
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

    public static void SetSession()
    {
        ActivityLog = new ActivityLogIem();

        #if UNITY_EDITOR
            SetSessionForEditor();
        #endif
    }

    static void SetSessionForEditor()
    {
        FirebasePR.InitializeFireBaseDb();
        if (CurrentPlayer.CampaignItem == null)
        {
            CurrentPlayer.CampaignItem = new CampaignItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 11, 10, 0, 0, 0, 0);
            CurrentPlayer.WorldRankItem = new WorldRankItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 0, 6, 0.62);
            ActivityLog = new ActivityLogIem();
            //CurrentPlayer.TrialMode = true;
        }
        MusicPR.SetVolumeSfx(1f);
        MusicPR.SetVolumeMusic(0.5f);
    }

    public static void SetTrialMode()
    {

        if (CurrentPlayer.CampaignItem != null)
            _campaignItemCopy = new CampaignItem(CurrentPlayer.CampaignItem.PlrId, CurrentPlayer.CampaignItem.PlrName
                , CurrentPlayer.CampaignItem.LvlNo, CurrentPlayer.CampaignItem.HitsCmp, CurrentPlayer.CampaignItem.Lives
                , CurrentPlayer.CampaignItem.ReacCmp, CurrentPlayer.CampaignItem.BnsTaken, CurrentPlayer.CampaignItem.BnsLastMlstn);
        CurrentPlayer.CampaignItem = new CampaignItem(string.Empty, string.Empty, 1, 0, 10, 0, 0, 0);
        TrialMode = true;
    }
    public static void EndTrialMode()
    {
        if (_campaignItemCopy != null)
            CurrentPlayer.CampaignItem = new CampaignItem(_campaignItemCopy.PlrId, _campaignItemCopy.PlrName, _campaignItemCopy.LvlNo, _campaignItemCopy.HitsCmp
            , _campaignItemCopy.Lives, _campaignItemCopy.ReacCmp, _campaignItemCopy.BnsTaken, _campaignItemCopy.BnsLastMlstn);
        TrialMode = false;
    }

}
