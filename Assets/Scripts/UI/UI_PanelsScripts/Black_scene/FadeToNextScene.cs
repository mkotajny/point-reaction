using UnityEngine;
using System.Collections;
using System.Threading;

public class FadeToNextScene : MonoBehaviour {
    public void Start()
    {
        Initiate.areWeFading = false;
        if (SessionVariables.CurrentScene == SessionVariables.PRScenes.MainMenu)
                Initiate.Fade("MainMenuScene", Color.black, 1.0f);
        else //QUIT
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
