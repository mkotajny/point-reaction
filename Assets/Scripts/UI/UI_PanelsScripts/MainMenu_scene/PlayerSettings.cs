using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour {

    Text _levelDropdownValue;
    Dropdown _levelDropDown;

    int _currentLevelNo, _bestLevelNo;

    void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();

        _currentLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("LevelNo"), 0);
        _bestLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("BestLevelNo"), -1);
        if (_bestLevelNo == -1)
            _bestLevelNo = _currentLevelNo;

        GenerateListOfOptions();
        _levelDropDown.value = _currentLevelNo;
    }

    void OnEnable()
    {
        ActivityLogger.AddLogLine("PLAYER SETTINGS menu panel has been opened");
    }


    void GenerateListOfOptions()
    {
        List<string> options = new List<string>();
        for (int i = 0; i <= _bestLevelNo; i++)
            options.Add(i.ToString());

        _levelDropDown.AddOptions(options);
    }

    public void SetDropdownValue()
    {
        PlayerPrefs.SetString("LevelNo", _levelDropdownValue.text);
    }
}
