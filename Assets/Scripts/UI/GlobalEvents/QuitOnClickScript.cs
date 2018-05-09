using UnityEngine;

public class QuitOnClickScript : MonoBehaviour {
    public void Quit()
    {
        ActivityLogger.AddLogLine("Player has LEFT THE GAME");
        ActivityLogger.SaveLog();

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;        
        #endif
    }
}
