using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    void Awake ()
    {
        ActivityLogger.InitializeLog();
        Screen.orientation = ScreenOrientation.Portrait;
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text
            = "Version " + Application.version;
	}

    void OnEnable()
    {
        //ActivityLogger.AddLogLine("MAIN MENU panel has been opened");
    }
}
