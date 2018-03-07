using UnityEngine;

public class Point {

    float _positionX, _positionZ;
    GameObject _pointGameObject, _gameObjectFactory;

    public float PositionX 
    {
        get { return _positionX; }
        set { _positionX = value; }
    }
    public float PositionZ
    {
        get { return _positionZ; }
        set { _positionZ = value; }
    }
    public GameObject PointGameObject
    {
        get { return _pointGameObject; }
        set { _pointGameObject = value; }
    }
    public GameObject GameObjectFactory
    {
        get { return _gameObjectFactory;}
        set { _gameObjectFactory = value; }
    }

    public Point (float positionX, float positionZ, GameObject gameObjectFactory)
    {
        _positionX = positionX;
        _positionZ = positionZ;
        _gameObjectFactory = gameObjectFactory;

        PointGameObject = 
            gameObjectFactory.GetComponent<GameObjectFactory>().GeneratePoint(_positionX, PositionZ);
        PointGameObject.SetActive(false);
    }
}

