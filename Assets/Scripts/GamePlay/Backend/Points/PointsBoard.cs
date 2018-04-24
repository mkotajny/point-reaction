using UnityEngine;
using System.Collections;

public class PointsBoard {

    Point[,] _points;
    public GameObject _activatedPoint;
    System.Random _randomizer;
    int _boardGranularity;
    int _pointSpawnTimeRange;
    GameObject _gameObjectFactory;

    public Point[,] Points
    {
        get { return _points; }
        set { _points = value; }
    }
    public int BoardGranurality
    {
        get { return _boardGranularity; }
        set { _boardGranularity = value; }
    }
    public int PointSpawnTimeRange
    {
        get { return _pointSpawnTimeRange;}
        set { _pointSpawnTimeRange = value; }
    }
    public GameObject GameObjectFactory
    {
        get { return _gameObjectFactory; }
        set { _gameObjectFactory = value; }
    }

    public GameObject ActivatedPoint { get { return _activatedPoint; } }

    public PointsBoard(float boardSizeX
        , float boardSizeZ
        , int boardGgranularity
        , int pointSpawnTimeRange
        , GameObject gameObjectFactory
        )
    {
        float siblingOffsetX = boardSizeX / (boardGgranularity + 1);
        float siblingOffsetZ = boardSizeZ / (boardGgranularity + 1);

        _randomizer = new System.Random();
        _gameObjectFactory = gameObjectFactory;
        _boardGranularity = boardGgranularity;
        _pointSpawnTimeRange = pointSpawnTimeRange;
        _points = new Point[_boardGranularity, _boardGranularity];

        for (int _z = 0; _z < boardGgranularity; _z++)
            for (int _x = 0; _x < boardGgranularity; _x++)
                Points[_x, _z] = 
                    new Point(boardSizeX/2 - siblingOffsetX * (_x + 1)
                    ,-boardSizeZ / 2 + siblingOffsetZ * (_z + 1)
                    , _gameObjectFactory);

    }

    public void ActivateOneOfPoints()
    {
        _activatedPoint = _points[_randomizer.Next(0, _boardGranularity)
            , _randomizer.Next(0, _boardGranularity)].PointGameObject;
        _activatedPoint.SetActive(true);
    }

    public void DeactivateActivePoint()
    {
        ActivatedPoint.SetActive(false);
    }
}
