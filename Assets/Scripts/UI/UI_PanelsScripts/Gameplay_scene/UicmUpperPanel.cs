using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UicmUpperPanel : MonoBehaviour {

    Text _textCampaignLives, _textCampaignLivesChange
        , _textLevelHits, _textLevelHitsChange
        , _textLevelFails, _textLevelFailsChange, _textAttemptResult;
    Color32 _winColor;
    UIContentManager _uiContentManager;

    void Awake() {
        _winColor = new Color32(50, 160, 50, 255);
        _uiContentManager = GameObject.Find("UIContentManager").GetComponent<UIContentManager>();
        _textCampaignLives = GameObject.Find("Text_lives").GetComponent<Text>();
        _textCampaignLivesChange = GameObject.Find("Text_lives_change").GetComponent<Text>();
        _textLevelHits = GameObject.Find("Text_hits").GetComponent<Text>();
        _textLevelHitsChange = GameObject.Find("Text_hits_change").GetComponent<Text>();
        _textLevelFails = GameObject.Find("Text_fails").GetComponent<Text>();
        _textLevelFailsChange = GameObject.Find("Text_fails_change").GetComponent<Text>();
        _textAttemptResult = GameObject.Find("Text_attemptResult").GetComponent<Text>();
        DeactivateChangeStats();
    }

    public void SetUpperPanelStats(GameLevel level)
    {
        _textCampaignLives.text = CurrentPlayer.CampaignItem.Lives.ToString();
        _textLevelHits.text = level.HitsQty.ToString() + "/10";
        _textLevelFails.text = _uiContentManager.GameMode_1.CurrentLevel.MissQty.ToString() + "/3";
    }

    void DeactivateChangeStats()
    {
        _textCampaignLivesChange.gameObject.SetActive(false);
        _textLevelFailsChange.gameObject.SetActive(false);
        _textLevelHitsChange.gameObject.SetActive(false);
        _textAttemptResult.gameObject.SetActive(false);
    }

    public IEnumerator ChangeUpperPanelStats(ScreenTouchTypes touchResult, GameLevel level)
    {
        if (level.HitsQty == 10 && level.MissQty == 0)
            CurrentPlayer.CampaignItem.Lives += level.BonusPerfectLevel;

        SetUpperPanelStats(level);
        if (touchResult == ScreenTouchTypes.NotTouched
            || touchResult == ScreenTouchTypes.Miss)
        {
            _textCampaignLivesChange.text = "(-1)";
            _textCampaignLivesChange.gameObject.SetActive(true);
            _textLevelFailsChange.text = "(+1)";
            _textLevelFailsChange.gameObject.SetActive(true);

            _textAttemptResult.color = Color.red;
            if (touchResult == ScreenTouchTypes.NotTouched)
                _textAttemptResult.text = "LATE";
            else
                _textAttemptResult.text = "MISS";
        }
        else if (touchResult == ScreenTouchTypes.Hit)
        {
            _textLevelHitsChange.text = "(+1)";
            _textLevelHitsChange.gameObject.SetActive(true);
            _textAttemptResult.color = _winColor;
            _textAttemptResult.text = "HIT";
            if (level.HitsQty == 10 && level.MissQty == 0)
            {
                _textCampaignLivesChange.text = "(+" + level.BonusPerfectLevel  + ")";
                _textCampaignLivesChange.gameObject.SetActive(true);
            }
        }
        _textAttemptResult.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        DeactivateChangeStats();
    }

    public IEnumerator ChangeUpperPanelStats(GameLevel level, int livesIncrease)
    {
        CurrentPlayer.CampaignItem.Lives += livesIncrease;

        SetUpperPanelStats(level);
        _textCampaignLivesChange.text = "(+" + livesIncrease.ToString() + ")";
        _textCampaignLivesChange.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        DeactivateChangeStats();
    }

}
