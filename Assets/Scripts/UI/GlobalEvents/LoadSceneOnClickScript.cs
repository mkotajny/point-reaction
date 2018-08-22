using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClickScript : MonoBehaviour {

    public void LoadByIndex (int sceneIndex)
    {
        ActivityLogger.SaveLog();
        SessionVariables.CurrentScene = SessionVariables.PRScenes.GameBoard;
        Initiate.Fade("BlackScene", Color.black, 1.0f);
    }
}
