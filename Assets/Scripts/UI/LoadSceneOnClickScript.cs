using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClickScript : MonoBehaviour {

    public void LoadByIndex (int sceneIndex)
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            Screen.orientation = ScreenOrientation.Portrait;
        else
            Screen.orientation = ScreenOrientation.LandscapeLeft;

        SceneManager.LoadScene(sceneIndex);
    }

}
