using UnityEngine;
using UnityEngine.UI;

// location of the Script: Unity Object tree --> UIContentManager --> BonusPanelScriptContainer
public class BonusPanelScriptContainer : MonoBehaviour {

    UIContentManager _uIContentManager;
    public Text _bonusValue, _bonusHeader;
    public GameControler _gameControler;
    int _backgroundAnimationIndex;


    public void ActivatePanel(UIContentManager uIContentManager)
    {
        int livesIncrease = 0;
        if (_uIContentManager == null)
            _uIContentManager = uIContentManager;
        _uIContentManager.GetBonusButton.gameObject.SetActive(false);
        livesIncrease = uIContentManager.GameMode_1.LivesBonuses[CurrentPlayer.CampaignItem.BnsTaken - 1];
        _backgroundAnimationIndex = _uIContentManager.Randomizer.Next(1, 3);
        _uIContentManager.backgrounds[_backgroundAnimationIndex].SetActive(true);
        _gameControler.AudioSources[5].Play();
        StartCoroutine(_uIContentManager.UpperPanel.ChangeUpperPanelStats(_uIContentManager.GameMode_1.CurrentLevel, livesIncrease));
        _bonusHeader.text = "Milestone Level Bonus no " + CurrentPlayer.CampaignItem.BnsTaken.ToString();
        _bonusValue.text = "+" + livesIncrease.ToString();
        try { _uIContentManager._zuiManager.OpenMenu("Menu_Bonus"); } catch { }
        _uIContentManager.GameMode_1.SaveToFireBase(false, _uIContentManager);
    }

    public void DeactivatePanel()
    {
        _uIContentManager.backgrounds[_backgroundAnimationIndex].SetActive(false);
        _uIContentManager.OpenLevelStartPanel();
    }
}
