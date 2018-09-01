using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;

public class PlayerSettings : MonoBehaviour {

    Text _levelDropdownValue, _playerName;
    Dropdown _levelDropDown;
    Button _signInButton, _signOutButton;
    int _currentLevelNo, _bestLevelNo;
    bool _sychronizeLewelDropdown = false;

    void Awake()
    {
        _levelDropdownValue = GameObject.Find("LevelDropdownLabel").GetComponent<Text>();
        _levelDropDown = GameObject.Find("LevelDropdown").GetComponent<Dropdown>();
        _signInButton = GameObject.Find("ButtonSignIn").GetComponent<Button>();
        _signOutButton = GameObject.Find("ButtonSignOut").GetComponent<Button>();
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
        if (_signInButton.gameObject.activeInHierarchy == CurrentPlayer.SignedIn
            && !_sychronizeLewelDropdown)
            SwitchGoogleSignButtons();
        if (_sychronizeLewelDropdown && !WorldRankPersister.LoadInProgress)  //World Rank Loaded (with new player data) after sign
        {
            SetNewPlayerData();
            _sychronizeLewelDropdown = false;
        }
    }

    public void GooglePlaySignInOut(bool signIn = true)
    {
        if (!CheckInternet.IsConnected() && signIn)
        {
            new Alert("No internet !", "Please connect with the internet and try again.")
                .SetPositiveButton("OK", () => {}).Show();
            return;
        }

        if (signIn)
        {
            _signInButton.interactable = false;
            CurrentPlayer.SignInGooglePlay();
            _sychronizeLewelDropdown = true;
            WorldRankPersister.LoadInProgress = true;
        }
        else
            CurrentPlayer.SignOutGooglePlay();
    }

    public void SwitchGoogleSignButtons()
    {
        if (CurrentPlayer.SignedIn)
            SetNewPlayerData();
        else
        {
            _playerName.text = "-";
            _signInButton.gameObject.SetActive(true);
            _signOutButton.gameObject.SetActive(false);
        }
    }
    void SetNewPlayerData()
    {
        _playerName.text = CurrentPlayer.PlayerName;
        _signInButton.interactable = true;
        _signInButton.gameObject.SetActive(false);
        _signOutButton.gameObject.SetActive(true);
        GenerateListOfLevels();
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

        _levelDropDown.ClearOptions();
        _levelDropDown.AddOptions(options);
        _levelDropDown.value = _currentLevelNo;
    }

    public void SetDropdownValue()
    {
        PlayerPrefs.SetInt("LevelNo", System.Convert.ToInt16(_levelDropdownValue.text));
    }
    #endregion
}
