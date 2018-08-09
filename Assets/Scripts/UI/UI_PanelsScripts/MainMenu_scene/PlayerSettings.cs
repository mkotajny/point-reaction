using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour {

    Text _levelDropdownValue, _playerName;
    Dropdown _levelDropDown;
    GameObject _signInButton, _signOutButton;
    int _currentLevelNo, _bestLevelNo;

    void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
        _signInButton = GameObject.Find("ButtonSignIn");
        _signOutButton = GameObject.Find("ButtonSignOut");
        _playerName = GameObject.Find("PlayerSettings_PlayerName_text").GetComponent<Text>();
    }

    private void OnEnable()
    {
        SwitchGoogleSignButtons();
        GenerateListOfLevels();
    }

    #region Google Play Account Management
    private void Update()
    {
        if (_signInButton.activeInHierarchy == CurrentPlayer.SignedIn)
            SwitchGoogleSignButtons();
    }

    public void GooglePlaySignInOut(bool signIn = true)
    {
        if (signIn)
            CurrentPlayer.SignInGooglePlay();
        else
            CurrentPlayer.SignOutGooglePlay();
    }

    public void SwitchGoogleSignButtons()
    {
        if (CurrentPlayer.SignedIn)
        {
            _playerName.text = CurrentPlayer.PlayerName;
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
        _currentLevelNo = PlayerPrefs.GetInt("LevelNo");
        _bestLevelNo = PlayerPrefs.GetInt("BestLevelNo");
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
        PlayerPrefs.SetInt("LevelNo", System.Convert.ToInt16(_levelDropdownValue.text));
    }
    #endregion
}
