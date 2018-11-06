using UnityEngine;
using UnityEngine.UI;


public class GameControler : MonoBehaviour {

    GameMode_1 _gameMode_1;
    public PointsBoard _pointsBoard;
    GameObject _gameObjectFactory, _gameBoard;
    Button _backToMainMenuButton;
    GameControlerTools _gameControlerTools;
    UIContentManager _uIContentManager;
    ScreenTouchTypes _screenTouch;
    public int PointsBoardGranurality = 10, PointSpawnTimeRange = 10;
    public AudioSource[] AudioSources;

    private void Awake()
    {
        SessionVariables.SetSessionForEditor();
    }

    void Start()
    {
        _gameMode_1 = new GameMode_1(this);
        _gameControlerTools = GameObject.Find("GameControlerTools").GetComponent<GameControlerTools>();
        _uIContentManager = GameObject.Find("UIContentManager").GetComponent<UIContentManager>();
        AudioSources = GameObject.Find("GameControler").GetComponents<AudioSource>();
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
            LoadMainMenuScene();

        if (_pointsBoard.ActivatedPoint != null 
            && _pointsBoard.ActivatedPoint.activeInHierarchy
            && !_gameMode_1.CurrentLevel.PointsLivingTimer.Active)
            _gameMode_1.CurrentLevel.PointsLivingTimer.Activate();

        if (_gameMode_1.CurrentLevel.PlayStatus != LevelPlayStatuses.InProgress
            && MusicPR.NextSongTimer.TimeElapsed())
            MusicPR.PlayNextSong(MusicPR.PlayListGameBoard);
    }

    public void FixedUpdate()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus != LevelPlayStatuses.InProgress)
            return;
        if (_gameMode_1.CurrentLevel.BetweenPointsTimer.TimeElapsed())
            _gameMode_1.ActivateSinglePoint();

        CheckPointTouch();

        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win
            || _gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Lost)
        {
            if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
            {
                AudioSources[4].Play(); //play fanfare
                CurrentPlayer.ActivityLog.Add(LogCategories.LevelPassed, CurrentPlayer.CampaignItem.PlrName + " has passed the level no " + CurrentPlayer.CampaignItem.LvlNo.ToString());
            }
            else CurrentPlayer.ActivityLog.Add(LogCategories.LevelFailed, CurrentPlayer.CampaignItem.PlrName + " has NOT passed the level no " + CurrentPlayer.CampaignItem.LvlNo.ToString());

            _uIContentManager.ActivateResultPanel(true);
        }
    }

    public void ActivateOneOfPoints()
    {
        _pointsBoard.ActivateOneOfPoints();
    }

    public void LevelStart()
    {
        _gameMode_1.SaveToFireBase(levelStart: true, uiContentManager: _uIContentManager);
        _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.InProgress;
        _gameMode_1.CurrentLevel.SpawnPoint();
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
            CurrentPlayer.LivesTaken--;
            _gameMode_1.CurrentLevel.RegisterFail(_screenTouch);
            _pointsBoard.DeactivateActivePoint();
            if (_screenTouch != ScreenTouchTypes.Miss)
                AudioSources[2].Play(); //fail sound

            StartCoroutine(_uIContentManager.UpperPanel.ChangeUpperPanelStats(_screenTouch, _gameMode_1.CurrentLevel));

            if (_gameMode_1.CurrentLevel.MissQty == _gameMode_1.CurrentLevel.MissesToLoose
                || CurrentPlayer.CampaignItem.Lives == 0)
                _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.Lost;
            else
                _gameMode_1.CurrentLevel.SpawnPoint();
        }

        if (_screenTouch == ScreenTouchTypes.Hit)
        {
            StartCoroutine(_uIContentManager.UpperPanel.ChangeUpperPanelStats(_screenTouch, _gameMode_1.CurrentLevel));
        }
    }
}
