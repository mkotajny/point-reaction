using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControler : MonoBehaviour {

    GameMode_1 _gameMode_1;
    PointsBoard PointsBoard;

    public GameObject GameObjectFactory, GameBoard, LevelStartPanel, LevelResultPanel;
    public Text PanelStartLevelValue, PanelResultLevelValue;
    public int PointsBoardGranurality = 10, PointSpawnTimeRange = 10;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        PointsBoard = new PointsBoard(GameBoard.transform.localScale.x
            , GameBoard.transform.localScale.y
            , PointsBoardGranurality
            , PointSpawnTimeRange
            , GameObjectFactory);

        _gameMode_1 = new GameMode_1(this, 0);
        OpenLevelStartPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void FixedUpdate()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.inProgress)
        {
            HandleTouch();
            CheckLevelStatus();
        }
    }

    public void OpenLevelStartPanel()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win)
            _gameMode_1.CurrentLevel = _gameMode_1.GameLevels[_gameMode_1.CurrentLevel.LevelNo + 1];

        PanelStartLevelValue.text = _gameMode_1.CurrentLevel.LevelNo.ToString();
        PanelResultLevelValue.text = _gameMode_1.CurrentLevel.LevelNo.ToString();
        LevelResultPanel.SetActive(false);
        LevelStartPanel.SetActive(true);
    }

    public void ActivateOneOfPoints()
    {
        StartCoroutine(PointsBoard.ActivateOneOfPoints());
    }

    public void LevelStart()
    {
        _gameMode_1.CurrentLevel.PlayStatus = LevelPlayStatuses.inProgress;
        _gameMode_1.ActivateSinglePoint(true);
    }

    public void HandleTouch()
    {
        int nbTouches = Input.touchCount;
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
                        _gameMode_1.RegisterHit();
                        hit.collider.gameObject.SetActive(false);
                        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.inProgress)
                            _gameMode_1.ActivateSinglePoint();
                    }
                }
            }
        }
    }

    public void CheckLevelStatus()
    {
        if (_gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Win
            || _gameMode_1.CurrentLevel.PlayStatus == LevelPlayStatuses.Lost)
            LevelResultPanel.SetActive(true);
    }
}
