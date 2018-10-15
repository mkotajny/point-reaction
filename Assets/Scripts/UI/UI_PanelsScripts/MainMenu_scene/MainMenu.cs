using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;

public class MainMenu : MonoBehaviour {

    public ZUIManager ZuiManager;
    public Text StartButtonText;
    public Button ButtonStart;

    public void OnEnable()
    {
        if (CurrentPlayer.CampaignItem != null
            && !string.IsNullOrEmpty(CurrentPlayer.CampaignItem.PlrName))
            StartButtonText.text = "ONLINE Game";
        else StartButtonText.text = "Game Start";
    }

    public void Update()
    {

        if (StartButtonText.text == "Game Start"
            && CurrentPlayer.CampaignItem != null
            && !string.IsNullOrEmpty(CurrentPlayer.CampaignItem.PlrName))
        {
            StartButtonText.text = "ONLINE Game";
            ButtonStart.interactable = true;
        }
    }

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
        if (CurrentPlayer.CampaignItem != null
            && !string.IsNullOrEmpty(CurrentPlayer.CampaignItem.PlrName))
        {
            if (CurrentPlayer.CampaignItem.IsNewCampaign())
            {
                GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
                return;
            }
            try { ZuiManager.OpenMenu("Menu_Start"); } catch { }
            return;
        }
        try { ZuiManager.OpenMenu("Menu_Play_Modes"); } catch { }
    }
}
