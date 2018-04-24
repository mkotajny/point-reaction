using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {

    GameMode_1 _gameMode_1;
    PointsBoard _pointsBoard;
    GameObject _gameObjectFactory, _gameBoard;
    GameControlerTools _gameControlerTools;
    UIContentManager _uIContentManager;
    public int PointsBoardGranurality = 10, PointSpawnTimeRange = 10;

    void Start()
    {
        _gameMode_1 = new GameMode_1(this, 0);
        _gameControlerTools = GameObject.Find("GameControlerTools").GetComponent<GameControlerTools>();
        _uIContentManager = GameObject.Find("UIContentManager").GetComponent<UIContentManager>();
        _uIContentManager.GameMode_1 = _gameMode_1;
        
        _gameObjectFactory = GameObject.Find("GameObjectFactory");
        _gameBoard = GameObject.Find("GameBoard");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        _pointsBoard = new PointsBoard(_gameBoard.transform.localScale.x
            , _gameBoard.transform.localScale.y
            , PointsBoardGranurality
            , PointSpawnTimeRange
            , _gameObjectFactory);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void FixedUpdate()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus != LevelPlayStatuses.inProgress)
            return;

        if (_gameMode_1.CurrentLevel.PointsLivingTimer.Active 
            && _gameControlerTools.IsPointTouched())
            _gameMode_1.RegisterHit();

        if (_gameMode_1.BetweenPointsTimer.TimeElapsed())
            _gameMode_1.ActivateSinglePoint();

        CheckPointLivingTime();

        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win
            || _gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Lost)
            _uIContentManager.ActivateResultPanel();
    }

    public void ActivateOneOfPoints()
    {
        _pointsBoard.ActivateOneOfPoints();
    }

    public void LevelStart()
    {
        _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.inProgress;
        _gameMode_1.BetweenPointsTimer.Activate();
    }

    void CheckPointLivingTime()
    {
        if (_gameMode_1.CurrentLevel.PointsLivingTimer.TimeElapsed())
        {
            _pointsBoard.DeactivateActivePoint();
            _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.Lost;
        }
    }
}
