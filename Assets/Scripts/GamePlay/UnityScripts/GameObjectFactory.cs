using UnityEngine;

public class GameObjectFactory : MonoBehaviour {

    public GameObject PointGameObject;
    Vector3 _position;
    Quaternion _rotation;

    private void Start()
    {
        _position = new Vector3();
        _rotation = new Quaternion();
    }

    public GameObject GeneratePoint(float pointPositionX, float pointPositionZ)
    {
        _position.x = pointPositionX;
        _position.z = pointPositionZ;
        return Instantiate(PointGameObject, _position, _rotation);
    }
}
