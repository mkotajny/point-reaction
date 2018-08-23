using UnityEngine;
using System.Collections;
using System.Threading;

public class FadeToNextScene : MonoBehaviour {
    public void Start()
    {
        Initiate.areWeFading = false;
        switch (SessionVariables.CurrentScene)
        {
            case SessionVariables.PRScenes.Quit:
                Application.Quit();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
                break;
            case SessionVariables.PRScenes.GameBoard:
                //Screen.orientation = ScreenOrientation.LandscapeLeft;
                Initiate.Fade("GameScene", Color.black, 1.0f);
                break;
            case SessionVariables.PRScenes.MainMenu:
                //Screen.orientation = ScreenOrientation.Portrait;
                Initiate.Fade("MainMenuScene", Color.black, 1.0f);
                break;
            default:
                break;
        }
    }

}
