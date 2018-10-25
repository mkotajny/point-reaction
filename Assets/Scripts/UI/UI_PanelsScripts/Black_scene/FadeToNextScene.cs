using UnityEngine;

public class FadeToNextScene : MonoBehaviour {
    public void Start()
    {
        Initiate.areWeFading = false;
        switch (SessionVariables.CurrentScene)  
        {
            case SessionVariables.PRScenes.GameLoading:
                Initiate.Fade("GameLoadingScene", Color.black, 1.0f);
                break;
            case SessionVariables.PRScenes.MainMenu:
                Initiate.Fade("MainMenuScene", Color.black, 1.0f);
                break;
            default:
                Application.Quit();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;
        }
    }
}
