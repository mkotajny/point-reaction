using UnityEngine;
using UnityEngine.UI;

public class BonusPanel : MonoBehaviour {

    UIContentManager _uIContentManager;
    public Text _bonusValue;
    public GameControler _gameControler;
    int _backgroundAnimationIndex;

    public void ActivatePanel(UIContentManager uIContentManager)
    {
        int livesIncrease;


        if (_uIContentManager == null)
            _uIContentManager = uIContentManager;

        livesIncrease = _uIContentManager.GameMode_1.GameLevels[_uIContentManager.GameMode_1.CurrentLevel.LevelNo - 2].BonusMileStoneLevel;
        _bonusValue.text = "+" + livesIncrease.ToString();
        _backgroundAnimationIndex = _uIContentManager.Randomizer.Next(1,3);
        _uIContentManager.backgrounds[_backgroundAnimationIndex].SetActive(true);

        StartCoroutine(_uIContentManager.UpperPanel.ChangeUpperPanelStats(_uIContentManager.GameMode_1.CurrentLevel, livesIncrease));
        CurrentPlayer.CampaignItem.LvlMilest = _uIContentManager.GameMode_1.CurrentLevel.LevelNo - 1;
        try { _uIContentManager._zuiManager.OpenMenu("Menu_Bonus"); } catch { }
        _gameControler.AudioSources[5].Play();
        _uIContentManager.GameMode_1.SaveToFireBase();
    }


    public void DeactivatePanel()
    {
        _uIContentManager.backgrounds[_backgroundAnimationIndex].SetActive(false);
        _uIContentManager.OpenLevelStartPanel();
    }
}
