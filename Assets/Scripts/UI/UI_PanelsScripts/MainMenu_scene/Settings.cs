using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    Toggle _vibrateToggle;
    Slider _musicVolume, _sfxVolume;
    AudioSource[] _sfxSounds;
    string _settingsValueText;
    bool afterFirstVolumeSet = false;

    void Awake()
    {
        _sfxSounds = GameObject.Find("Panel_Settings").GetComponents<AudioSource>();
        _vibrateToggle = GameObject.Find("VibrateToggle").GetComponent<Toggle>();
        _musicVolume = GameObject.Find("Music_Slider_Volume").GetComponent<Slider>();
        _sfxVolume = GameObject.Find("SFX_Slider_Volume").GetComponent<Slider>();
        _musicVolume.value = MusicPR.VolumeMusic * 100;
        _sfxVolume.value = MusicPR.VolumeSfx * 100;
        GetVibrate();
    }

    public void SetVibrate()
    {
        if (_vibrateToggle.isOn)
            PlayerPrefs.SetString("Vibrate", "1");
        else
            PlayerPrefs.SetString("Vibrate", "0");
    }
    public void GetVibrate()
    {
        _settingsValueText = PlayerPrefs.GetString("Vibrate");
        if (_settingsValueText == "1")
            _vibrateToggle.isOn = true;
        else
            _vibrateToggle.isOn = false;
    }

    public void SetNewMusicVolume()
    {
        MusicPR.SetVolumeMusic(_musicVolume.value/100);
    }

    public void SetNewSfxVolume()
    {
        MusicPR.SetVolumeSfx(_sfxVolume.value / 100);
        _sfxSounds[0].volume = MusicPR.VolumeSfx;
        if (!afterFirstVolumeSet)
            afterFirstVolumeSet = true;
        else
            _sfxSounds[0].Play(10000);
    }
}
