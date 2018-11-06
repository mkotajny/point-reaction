
public static class SessionVariables  {
    public enum PRScenes
    {
        GameLoading,
        MainMenu,
        GameBoard,
        Quit
    }

    static PRScenes _currentScene;

    public static PRScenes CurrentScene
    {
        get { return _currentScene; }
        set { _currentScene = value; }
    }

    public static void SetSessionForEditor()
    {
#if UNITY_EDITOR
        FirebasePR.InitializeFireBaseDb();
        if (CurrentPlayer.CampaignItem == null)
        {
            CurrentPlayer.CampaignItem = new CampaignItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 11, 10, 0, 0, 0, 0);
            CurrentPlayer.WorldRankItem = new WorldRankItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 0, 6, 0.62);
            CurrentPlayer.ActivityLog = new ActivityLogIem();
            //CurrentPlayer.TrialMode = true;
        }

        MusicPR.SetVolumeSfx(1f);
        MusicPR.SetVolumeMusic(0.5f);
#endif
    }
}
