using PaperPlaneTools;
using UnityEngine;

public class PlayModesMenu : MonoBehaviour {

    public ZUIManager ZuiManager;

    public void RunTrialMode()
    {
        SessionVariables.SetTrialMode();
        GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
    }

    public void StartRealGame()
    {
        if (CurrentPlayer.CampaignItem == null
            || CurrentPlayer.CampaignItem.PlrName == string.Empty)
        {
            new Alert("You are not Signed in !", "Please Sign in and try again\n\n(\"Main Menu\" --> \"Player\" --> \"Sign in\").")
                .SetPositiveButton("OK", () => { }).Show();
            return;
        }

        if (CurrentPlayer.CampaignItem != null
            && CurrentPlayer.CampaignItem.IsNewCampaign())
        {
            GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
            return;
        }
        try { ZuiManager.OpenMenu("Menu_Start"); } catch { }
    }
}
