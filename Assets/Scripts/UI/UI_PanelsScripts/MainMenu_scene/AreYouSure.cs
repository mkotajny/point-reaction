using UnityEngine;

public class AreYouSure : MonoBehaviour {

    public void ResetAndStartNewCampaign()
    {
        CurrentPlayer.CampaignItem.ResetCampaign();
        GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
    }
}
