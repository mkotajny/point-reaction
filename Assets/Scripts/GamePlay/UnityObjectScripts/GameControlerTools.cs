﻿using UnityEngine;

public enum ScreenTouchTypes
{
    NotTouched = 0,
    Miss = 1,
    Hit = 2
}

public class GameControlerTools : MonoBehaviour {

    AudioSource[] _audioSources;
    public GameObject _explosion;

    public void Start()
    {
        _audioSources = GameObject.Find("GameControler").GetComponents<AudioSource>();
        foreach (AudioSource audiosource in _audioSources)
            audiosource.volume = MusicPR.VolumeSfx;
    }

    public ScreenTouchTypes ScreenTouched(GameMode_1 gameMode_1)
    {
        int nbTouches = Input.touchCount;
        float timeElapsed;
        if (nbTouches > 0)
        {
            for (int i = 0; i < nbTouches; i++)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began
                    || touch.phase == TouchPhase.Moved
                    || touch.phase == TouchPhase.Stationary)
                {
                    Ray screenRay = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(screenRay, out hit))
                    {
                        if (hit.collider.gameObject.name == "GameBoard")
                        {
                            _audioSources[1].Play(); //miss sound
                            return ScreenTouchTypes.Miss;
                        }
                        if (hit.collider.gameObject.name == "SubBoard")
                            return ScreenTouchTypes.NotTouched;
                        
                        //Point touched too quickly
                        timeElapsed = gameMode_1.CurrentLevel.PointsLivingTimer.TooQuick(0.1f);
                        if (hit.collider.gameObject.name == "ThePoint(Clone)" && timeElapsed != 0)
                        {
                            _audioSources[3].Play(); //false start sound
                            return ScreenTouchTypes.Miss;
                        }

                        //Point hit
                        Instantiate(_explosion
                            , hit.collider.gameObject.transform.position
                            , hit.collider.gameObject.transform.rotation);

                        hit.collider.gameObject.SetActive(false);
                        gameMode_1.CurrentLevel.RegisterHit(gameMode_1.CurrentLevel.HitsToWin, Time.time);
                        _audioSources[0].Play();  //shot sound
                        if (GameOptions.Vibrate)
                            Handheld.Vibrate();
                        return ScreenTouchTypes.Hit;
                    }
                }
            }
        }
        return ScreenTouchTypes.NotTouched ;
    }
}
