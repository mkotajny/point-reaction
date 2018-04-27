using UnityEngine;
using UnityEngine.UI;

public class MainMenuStart : MonoBehaviour {

    void Start ()
    {        
        Screen.orientation = ScreenOrientation.Portrait;
        GameObject.Find("VersionNumber_text").GetComponent<Text>().text 
            = "Version " + Application.version;
    }	
}
