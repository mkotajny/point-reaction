using System.Collections.Generic;
using UnityEngine;
using PaperPlaneTools;

public class ForTesters : MonoBehaviour {

    void OnEnable()
    {
        FirebasePR.InitializeFireBaseDb();
        //UpdateWorldRank("MMzIVx7Fs0SlKY6VqQqlcFIbtHQ2", "marekkoszmarek", 29, 8, 0.36);
        /*UpdateWorldRank("2", "Zenek", 31, 2, 0.92);
        UpdateWorldRank("3", "Stefan", 17, 2, 0.82);*/

        //WorldRankPersister.LoadWorldRank();
        //FirebasePR.SignOutFireBase();

        //GameLevelPersister.ListAllPlayerPrefsValues();

        /*new Alert("No internet !", "Please connect with internet and try again.")
            .SetPositiveButton("OK", () => {})
            .Show();*/

        //PlayerPrefs.SetInt("InGooglePlay", 0);
        //PlayerPrefs.SetInt("ScoreServerUpdated", 0);

        //MusicManager.play("Music/Menu/352428__klavo1985__mgs-sample-loop-by-kk-v2");


        //GameLevelPersister.ListAllPlayerPrefsValues();

        /*
        string json = JsonUtility.ToJson(new CampaignItem("1"
            , "zenek"
            , 2
            , 7
            , System.Convert.ToDouble(0.37)));

        FirebasePR.CampaignDbReference.Child("zenek").SetRawJsonValueAsync(json);
        */
    }

    public void ActivityLogPreview()
    {
        //ActivityLogger.PreviewLog();
        CurrentPlayer.GetCurrentPlayerData();
    }

}
