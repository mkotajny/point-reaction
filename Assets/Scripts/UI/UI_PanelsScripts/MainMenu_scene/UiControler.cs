using UnityEngine;
using UnityEngine.UI;


public class UiControler : MonoBehaviour {


    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ActivityLogger.InitializeLog();
        GameLevelPersister.LevelLoad();

        GameObject.Find("VersionNumber_text").GetComponent<Text>().text
        = "Version " + Application.version;

        //FirebasePR.InitializeGooglePlay();
        FirebasePR.InitializeFireBaseDb();
        if (PlayerPrefs.GetInt("InGooglePlay") == 1)
            CurrentPlayer.SignInFireBase();
    }
}