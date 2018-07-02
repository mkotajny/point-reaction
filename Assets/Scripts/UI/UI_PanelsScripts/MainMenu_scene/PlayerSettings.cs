using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour {

    Text _levelDropdownValue, _playerName;
    Dropdown _levelDropDown;
    UiControler _uiControler;
    GameObject _signInButton, _signOutButton;

    bool PlayerAuthenticated
    {
        get
        {
            return (string.IsNullOrEmpty(_uiControler.PlayerName.text)) ? false : true;
        }
    }

    int _currentLevelNo, _bestLevelNo;

    void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
        _uiControler = GameObject.Find("UiControler").GetComponent<UiControler>();
        _signInButton = GameObject.Find("ButtonSignIn");
        _signOutButton = GameObject.Find("ButtonSignOut");
        _playerName = GameObject.Find("PlayerSettings_PlayerName_text").GetComponent<Text>();

        SwitchGoogleSignButtons();
        GenerateListOfLevels();
    }

    #region Google Play Account Management


    private void Update()
    {
        if (_signInButton.activeInHierarchy == PlayerAuthenticated)
            SwitchGoogleSignButtons();
    }

    public void GooglePlaySignInOut(bool signIn = true)
    {
        if (signIn)
            _uiControler.GooglePlaySignIn();
        else
            _uiControler.GooglePlaySignOut();
    }

    public void SwitchGoogleSignButtons()
    {
        if (PlayerAuthenticated)
        {
            _playerName.text = _uiControler.PlayerName.text;
            _signInButton.SetActive(false);
        }
        else
        {
            _playerName.text = "-";
            _signInButton.SetActive(true);
        }

        _signOutButton.SetActive(!_signInButton.activeInHierarchy);
    }
    #endregion

    #region Current Level Selection
    void GenerateListOfLevels()
    {
        _currentLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("LevelNo"), 0);
        _bestLevelNo = ConvertionTools.GestIntFromString(PlayerPrefs.GetString("BestLevelNo"), -1);
        if (_bestLevelNo == -1)
            _bestLevelNo = _currentLevelNo;

        List<string> options = new List<string>();
        for (int i = 0; i <= _bestLevelNo; i++)
            options.Add(i.ToString());

        _levelDropDown.AddOptions(options);
        _levelDropDown.value = _currentLevelNo;
    }

    public void SetDropdownValue()
    {
        PlayerPrefs.SetString("LevelNo", _levelDropdownValue.text);
    }
    #endregion
}
