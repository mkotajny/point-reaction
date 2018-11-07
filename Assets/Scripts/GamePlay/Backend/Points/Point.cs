using UnityEngine;

public class Point
{
    private readonly GameObject _gameObjectFactory;

    public float PositionX { get; set; }
    public float PositionZ { get; set; }
    public GameObject PointGameObject { get; private set; }
    public GameObject GameObjectFactory
    {
        get { return _gameObjectFactory;}
    }

    public Point (float positionX, float positionZ, GameObject gameObjectFactory)
    {
        PositionX = positionX;
        PositionZ = positionZ;
        _gameObjectFactory = gameObjectFactory;

        PointGameObject = 
            gameObjectFactory.GetComponent<GameObjectFactory>().GeneratePoint(PositionX, PositionZ);
        PointGameObject.SetActive(false);
    }
}
