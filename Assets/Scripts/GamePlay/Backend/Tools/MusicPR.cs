using System.Collections.Generic;
using UnityEngine;

public static class MusicPR  {

    static List<string> _playListMenu = new List<string>();
    static List<string> _playListGameBoard = new List<string>();
    static string _currentStrong;
    static float _volumeMusic = -1f, _volumeSfx = -1f;

    public static List<string> PlayListMenu { get { return _playListMenu; } }
    public static List<string> PlayListGameBoard { get { return _playListGameBoard; } }
    public static Timer NextSongTimer { get; private set; }
    public static float VolumeMusic { get { return _volumeMusic; } }
    public static float VolumeSfx { get { return _volumeSfx; } }


    public static void LoadList(List<string> playList)
    {
        string suffix, newSong;
        List<string> fileNames = new List<string>();

        if (_playListMenu.Count > 0 && _playListGameBoard.Count > 0)
            return;

        if (playList == _playListMenu)
        {
            suffix = "Menu";
            fileNames.Add("Blue Line Loop");
        }
        else
        {
            suffix = "GameBoard";
            fileNames.Add("174589__dingo1__down-dark-metal-electronic-loop");
            fileNames.Add("369066__mrthenoronha__hurry-loop");
            fileNames.Add("428858__supervanz__duskwalkin-loop");
            fileNames.Add("DNB2 166");
            fileNames.Add("Jumper Boy");
        }

        playList.Clear();

        foreach (string f in fileNames)
        {
            newSong = "Music/" + suffix + "/" + f;
            if (newSong != _currentStrong)
                playList.Add(newSong);
        }
       
        if (NextSongTimer == null) NextSongTimer = new Timer(120, false);
        if (_volumeMusic == -1f) SetVolumeMusic();
    }

    public static void PlayNextSong(List<string> playList)
    {
        if (playList.Count == 0)
            LoadList(playList);

        int nextSongNumber = new System.Random().Next(0, playList.Count);

        MusicManager.play(playList[nextSongNumber], 2.0f, 2.0f);
        _currentStrong = playList[nextSongNumber];
        playList.RemoveAt(nextSongNumber);
        NextSongTimer.Activate();
        MusicManager.setVolume(_volumeMusic);
    }

    public static void SetVolumeMusic(float volume = -1f)
    {
        if (volume != -1f)
        {
            _volumeMusic = volume;
            PlayerPrefs.SetString("MusicVolume", _volumeMusic.ToString());
        }
        else
        {
            string volumeFromDisk = PlayerPrefs.GetString("MusicVolume");
            if (string.IsNullOrEmpty(volumeFromDisk))
                volumeFromDisk = "1";
            _volumeMusic = (float)System.Convert.ToDouble(volumeFromDisk);
        }
        MusicManager.setVolume(_volumeMusic);
    }

    public static void SetVolumeSfx(float volume = -1f)
    {
        if (volume != -1f)
        {
            _volumeSfx = volume;
            PlayerPrefs.SetString("SfxVolume", _volumeSfx.ToString());
        }
        else
        {
            string volumeFromDisk = PlayerPrefs.GetString("SfxVolume");
            if (string.IsNullOrEmpty(volumeFromDisk))
                volumeFromDisk = "1";
            _volumeSfx = (float)System.Convert.ToDouble(volumeFromDisk);
        }
    }
}
