using UnityEngine;

public class LoadSceneOnClickScript : MonoBehaviour {

    public void LoadByIndex (int sceneIndex)
    {
        SessionVariables.CurrentScene = SessionVariables.PRScenes.GameBoard;
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
}
