
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
}
