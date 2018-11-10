using UnityEngine;
using UnityEngine.UI;
using PaperPlaneTools;

public class PlayerSettings : MonoBehaviour {

    public GameObject ModalPanel;
    Text _playerName;
    Button _signInButton, _signOutButton;

    void Awake()
    {
        _signInButton = GameObject.Find("ButtonSignIn").GetComponent<Button>();
        _signOutButton = GameObject.Find("ButtonSignOut").GetComponent<Button>();
        _playerName = GameObject.Find("PlayerSettings_PlayerName_text").GetComponent<Text>();
    }

    private void OnEnable()
    {
        SwitchGoogleSignButtons();
    }

    private void Update()
    {
        if (_signInButton.gameObject.activeInHierarchy == CurrentPlayer.SignedIn)
            SwitchGoogleSignButtons();

        if (ProgressBarPR.ProgressStatus == ProgressBarPrStatuses.Succeded)
        {
            ModalPanel.SetActive(false);
            ProgressBarPR.Deactivate();
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
            ModalPanel.SetActive(true);
            ProgressBarPR.Activate("Loading ...", 4);
            CurrentPlayer.SignInGooglePlay();
        }
        else
            CurrentPlayer.SignOutGooglePlay();
    }

    public void SwitchGoogleSignButtons()
    {
        if (CurrentPlayer.SignedIn)
        {
            _playerName.text = CurrentPlayer.CampaignItem.PlrName;
            _signInButton.gameObject.SetActive(false);
            _signOutButton.gameObject.SetActive(true);
        }
        else
        {
            _playerName.text = "-";
            _signInButton.gameObject.SetActive(true);
            _signOutButton.gameObject.SetActive(false);
        }
    }
}
