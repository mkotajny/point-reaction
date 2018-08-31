using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {

    GameMode_1 _gameMode_1;
    public PointsBoard _pointsBoard;
    GameObject _gameObjectFactory, _gameBoard;
    Button _backToMainMenuButton;
    GameControlerTools _gameControlerTools;
    UIContentManager _uIContentManager;
    AudioSource[] _audioSources;
    ScreenTouchTypes _screenTouch;
    public int PointsBoardGranurality = 10, PointSpawnTimeRange = 10;

    
    void Start()
    {
        //GameLevelPersister.ResetGame();
        ActivityLogger.InitializeLog();
        _gameMode_1 = new GameMode_1(this);
        _gameControlerTools = GameObject.Find("GameControlerTools").GetComponent<GameControlerTools>();
        _uIContentManager = GameObject.Find("UIContentManager").GetComponent<UIContentManager>();
        _audioSources = GameObject.Find("GameControler").GetComponents<AudioSource>();
        _backToMainMenuButton = GameObject.Find("ButtonBlue_Back").GetComponent<Button>();
        _uIContentManager.GameMode_1 = _gameMode_1;
        GameOptions.LoadOptions();
        
        _gameObjectFactory = GameObject.Find("GameObjectFactory");
        _gameBoard = GameObject.Find("GameBoard");
        _pointsBoard = new PointsBoard(_gameBoard.transform.localScale.x + 3
            , _gameBoard.transform.localScale.y + 1
            , PointsBoardGranurality
            , PointSpawnTimeRange
            , _gameObjectFactory);

        _uIContentManager.OpenLevelStartPanel();
        MusicPR.SetVolumeMusic();
        MusicPR.PlayNextSong(MusicPR.PlayListGameBoard);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenuScene();
            ActivityLogger.SaveLog();
        }

        if (_pointsBoard.ActivatedPoint != null 
            && _pointsBoard.ActivatedPoint.activeInHierarchy
            && !_gameMode_1.CurrentLevel.PointsLivingTimer.Active)
            _gameMode_1.CurrentLevel.PointsLivingTimer.Activate();

        if (_gameMode_1.CurrentLevel.PlayStatus != LevelPlayStatuses.inProgress
            && MusicPR.NextSongTimer.TimeElapsed())
            MusicPR.PlayNextSong(MusicPR.PlayListGameBoard);
    }

    public void FixedUpdate()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus != LevelPlayStatuses.inProgress)
            return;

        if (_gameMode_1.CurrentLevel.BetweenPointsTimer.TimeElapsed())
            _gameMode_1.ActivateSinglePoint();

        CheckPointTouch();

        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win
            || _gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Lost)
        {
                _uIContentManager.ActivateResultPanel();
            if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
                _audioSources[4].Play(); //play fanfare
        }
    }

    public void ActivateOneOfPoints()
    {
        _pointsBoard.ActivateOneOfPoints();
    }

    public void LevelStart()
    {
        _gameMode_1.CurrentLevel.Restart();
        _backToMainMenuButton.gameObject.SetActive(false);
    }

    public void LoadMainMenuScene()
    {
        SessionVariables.CurrentScene = SessionVariables.PRScenes.MainMenu;
        Initiate.Fade("MainMenuScene", Color.black, 1.0f);
    }


    void CheckPointTouch()
    {
        if (!_gameMode_1.CurrentLevel.PointsLivingTimer.Active)
            return;

        _screenTouch = _gameControlerTools.ScreenTouched(_gameMode_1);

        if (_gameMode_1.CurrentLevel.PointsLivingTimer.TimeElapsed()
            || _screenTouch == ScreenTouchTypes.Miss)
        {
            _pointsBoard.DeactivateActivePoint();
            _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.Lost;
            if (_screenTouch != ScreenTouchTypes.Miss)
            {
                _audioSources[2].Play(); //fail sound
                ActivityLogger.AddLogLine("Point no "
                        + (_gameMode_1.CurrentLevel.HitsQty + 1)
                        + " NOT TOUCHED (time elapsed)");
            }
            else
                ActivityLogger.AddLogLine("Point no "
                    + (_gameMode_1.CurrentLevel.HitsQty + 1) + " MISSED");
        }
    }
}
