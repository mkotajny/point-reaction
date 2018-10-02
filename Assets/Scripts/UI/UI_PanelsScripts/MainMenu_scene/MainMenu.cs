using UnityEngine;
using PaperPlaneTools;

public class MainMenu : MonoBehaviour {

    public ZUIManager ZuiManager;

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

    public void StartNewGame()
    {

#if !UNITY_EDITOR

        if (CurrentPlayer.CampaignItem == null)
        {
            new Alert("You are not Signed in !", "Please Sign in and try again  (press \"Player\" --> \"Player Settings\" --> \"Sign in\").")
                .SetPositiveButton("OK", () => { }).Show();

            return;
        }
#else 
        if (CurrentPlayer.CampaignItem == null)
            CurrentPlayer.CampaignItem = new CampaignItem("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 1, 0, 5, 0, 0, 0);
#endif


        if (CurrentPlayer.CampaignItem.IsNewCampaign())
        {
            GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
            return;
        }

        try { ZuiManager.OpenMenu("Menu_Start"); } catch { }
    }
}
