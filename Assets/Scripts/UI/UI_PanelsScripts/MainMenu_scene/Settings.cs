using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    Toggle _vibrateToggle;
    string _settingsValueText;

    void Awake()
    {
        _vibrateToggle = GameObject.Find("VibrateToggle").GetComponent<Toggle>();
        GetVibrate();
    }

    void OnEnable()
    {
        //ActivityLogger.AddLogLine("SETTINGS panel has been opened");
    }


    public void SetVibrate()
    {
        if (_vibrateToggle.isOn)
            PlayerPrefs.SetString("Vibrate", "1");
        else
            PlayerPrefs.SetString("Vibrate", "0");
    }
    public void GetVibrate()
    {
        _settingsValueText = PlayerPrefs.GetString("Vibrate");
        if (_settingsValueText == "1" || string.IsNullOrEmpty(_settingsValueText))
            _vibrateToggle.isOn = true;
        else
            _vibrateToggle.isOn = false;
    }
}
