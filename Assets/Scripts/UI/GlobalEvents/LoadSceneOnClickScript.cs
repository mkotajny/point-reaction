using UnityEngine;
using PaperPlaneTools;

public class LoadSceneOnClickScript : MonoBehaviour {

    public void LoadByIndex (int sceneIndex)
    {
#if !UNITY_EDITOR

        if (string.IsNullOrEmpty(CurrentPlayer.PlayerName))
        {
            new Alert("You are not Signed in !", "Please Sign in and try again  (press \"Player\" --> \"Player Settings\" --> \"Sign in\").")
                .SetPositiveButton("OK", () => { }).Show();

            return;
        }
#endif
        SessionVariables.CurrentScene = SessionVariables.PRScenes.GameBoard;
        Initiate.Fade("GameScene", Color.black, 1.0f);
    }
}
