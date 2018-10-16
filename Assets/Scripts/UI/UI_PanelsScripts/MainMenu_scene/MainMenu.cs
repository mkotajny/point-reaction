using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;

public class MainMenu : MonoBehaviour {

    public ZUIManager ZuiManager;
    public Text StartButtonText;
    public Button ButtonStart;

    public void ForwardToWorldRank()
    {
        if (!CheckInternet.IsConnected())
        {
            new Alert("No internet !", "Please connect with the internet and try again.")
                .SetPositiveButton("OK", () => { }).Show();
        }
        else
            ZuiManager.OpenMenu("Menu_WorldRank");
    }
}
