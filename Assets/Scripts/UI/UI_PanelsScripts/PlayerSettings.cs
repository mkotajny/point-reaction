using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour {

    Text _levelDropdownValue;
    Dropdown _levelDropDown;
    string _settingsValueText;

    public void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
        GetDropdownValue();
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

}
