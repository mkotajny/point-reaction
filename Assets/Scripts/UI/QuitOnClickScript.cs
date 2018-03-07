using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClickScript : MonoBehaviour {
    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;        
        #endif
    }
}
