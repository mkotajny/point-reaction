using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public Text FinalResult;
    public UIContentManager uIContentManager;

    private void OnEnable()
    {
        FinalResult.text = CurrentPlayer.CampaignItem.LvlNo.ToString();
        uIContentManager.GameMode_1.GameOver();
    }
}
