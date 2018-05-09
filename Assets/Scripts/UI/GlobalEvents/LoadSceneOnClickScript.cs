using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClickScript : MonoBehaviour {

    public void LoadByIndex (int sceneIndex)
    {
        ActivityLogger.SaveLog();
        SceneManager.LoadScene(sceneIndex);
    }
}
