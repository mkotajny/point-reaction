using UnityEngine;

public class QuitOnClickScript : MonoBehaviour {
    public void Quit()
    {
        SessionVariables.CurrentScene = SessionVariables.PRScenes.Quit;
        Initiate.Fade("BlackScene", Color.black, 1.0f);
    }
}
