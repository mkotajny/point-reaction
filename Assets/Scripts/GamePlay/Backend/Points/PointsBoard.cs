using UnityEngine;


public class PointsBoard {
    public GameObject _activatedPoint;
    private System.Random _randomizer;

    public Point[,] Points { get; set; }
    public int BoardGranurality { get; set; }
    public int PointSpawnTimeRange { get; set; }
    public GameObject GameObjectFactory { get; set; }

    public GameObject ActivatedPoint { get { return _activatedPoint; } }

    public PointsBoard(float boardSizeX
        , float boardSizeZ
        , int boardGgranularity
        , int pointSpawnTimeRange
        , GameObject gameObjectFactory
        )
    {
        float siblingOffsetX = boardSizeX / (boardGgranularity + 1.5f);
        float siblingOffsetZ = boardSizeZ / (boardGgranularity + 1);

        _randomizer = new System.Random();
        GameObjectFactory = gameObjectFactory;
        BoardGranurality = boardGgranularity;
        PointSpawnTimeRange = pointSpawnTimeRange;
        Points = new Point[BoardGranurality, BoardGranurality];

        for (int _z = 0; _z < boardGgranularity; _z++)
            for (int _x = 0; _x < boardGgranularity; _x++)
                Points[_x, _z] =
                    new Point(boardSizeX / 2 - (siblingOffsetX) * (_x + 1)
                    , -boardSizeZ / 2 + siblingOffsetZ * (_z + 1)
                    , GameObjectFactory);

    }

    public void ActivateOneOfPoints()
    {
        _activatedPoint = Points[_randomizer.Next(0, BoardGranurality)
            , _randomizer.Next(0, BoardGranurality)].PointGameObject;
        _activatedPoint.SetActive(true);
    }

    public void DeactivateActivePoint()
    {
        ActivatedPoint.SetActive(false);
    }
}
