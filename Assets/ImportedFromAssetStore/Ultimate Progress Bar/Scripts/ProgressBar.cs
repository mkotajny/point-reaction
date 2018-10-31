using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public GameObject ModalPanel, ContinueButton;
    public Image BarBody, BarSlider;
	public Text LoadingPercent, ProgressComment;
    float _fillSpeed;

    private void OnEnable()
    {
        BarSlider.fillAmount = 0;
        ProgressComment.text = string.Empty;
    }

    // Update is called once per frame
    void Update () {
        if (ProgressComment.text != ProgressBarPR.StatusComment)
            ProgressComment.text = ProgressBarPR.StatusComment;

        if (BarSlider.fillAmount >= 1)
        {
            if (ProgressBarPR.CurrentProgressValue == 1)
                ProgressBarPR.SetSuccess();
            else
                ProgressBarPR.SetFail("problems has been occurred during sign in.");
        }


        switch (ProgressBarPR.ProgressStatus)
        {
            case ProgressBarPrStatuses.InProgress:
                
                _fillSpeed = BarSlider.fillAmount < ProgressBarPR.CurrentProgressValue ? 0.5f : 0.05f * (1f - ProgressBarPR.CurrentProgressValue + 0.01f);
                BarSlider.fillAmount += Application.GetStreamProgressForLevel(0) * Time.deltaTime * _fillSpeed;
                float v = BarSlider.fillAmount * 100;
                LoadingPercent.text = "" + v.ToString("F0") + " %";
                break;
            case ProgressBarPrStatuses.Failed:
                ContinueButton.SetActive(true);
                break;
            default:
                break;
        }
                    
    }

    public void CloseWindow()
    {
        ContinueButton.SetActive(false);
        ModalPanel.SetActive(false);
    }
}
