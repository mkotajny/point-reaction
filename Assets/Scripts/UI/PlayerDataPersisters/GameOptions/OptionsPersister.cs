using UnityEngine;
using UnityEngine.UI;

public class OptionsPersister : MonoBehaviour {

    Text _levelDropdownValue;
    Dropdown _levelDropDown;
    Toggle _vibrateToggle;
    string _settingsValueText;

    public void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
        _vibrateToggle = GameObject.Find("VibrateToggle").GetComponent<Toggle>();
        GetDropdownValue();
        GetVibrate();
    }

    public void SetDropdownValue()
    {
        PlayerPrefs.SetString("Game Difficulty", _levelDropdownValue.text);
    }
    public void GetDropdownValue()
    {
        _settingsValueText = PlayerPrefs.GetString("Game Difficulty");
        int dropDownValue;
        int.TryParse(_settingsValueText, out dropDownValue);
        _levelDropDown.value = dropDownValue - 1;
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
