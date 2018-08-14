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


        GameLevelPersister.ListAllPlayerPrefsValues();
        


    }

    public void ActivityLogPreview()
    {
        PlayerPrefs.DeleteKey("PlayerId");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("ScoreServerUpdated");
        PlayerPrefs.DeleteKey("InGooglePlay");
        //ActivityLogger.PreviewLog();
    }

    public void UpdateWorldRank(string playerId
        , string playerName
        , int levelNo
        , int pointsHit
        , double reactionAvg)
    {
        string json;
        WorldRankItem worldRankItem = new WorldRankItem(playerId, playerName, levelNo
            , pointsHit, reactionAvg);

        json = JsonUtility.ToJson(worldRankItem);
        FirebasePR.FirebaseDbReference.Child("topreactors")
            .Child(playerId).SetRawJsonValueAsync(json);
    }

}
