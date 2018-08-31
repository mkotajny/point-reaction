using UnityEngine;
using PaperPlaneTools;

public class MainMenu : MonoBehaviour {

    public ZUIManager zuiManager;

    public void ForwardToWorldRank()
    {
        if (!CheckInternet.IsConnected())
        {
            new Alert("No internet !", "Please connect with the internet and try again.")
                .SetPositiveButton("OK", () => { }).Show();
        }
        else
            zuiManager.OpenMenu("Menu_WorldRank");
    }

}
