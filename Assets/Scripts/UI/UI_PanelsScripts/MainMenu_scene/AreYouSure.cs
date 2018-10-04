using UnityEngine;

public class AreYouSure : MonoBehaviour {

    public void ResetAndStartNewCampaign()
    {
        CurrentPlayer.CampaignsHistoryItem.EndOfCampaignIntoToFirebase();
        CurrentPlayer.CampaignItem.ResetCampaign();
        CurrentPlayer.CampaignItem.SaveToFirebase(deleteRow: true);
        GetComponent<LoadSceneOnClickScript>().LoadByIndex(1);
    }
}
