using UnityEngine;

public enum ScreenTouchTypes
{
    NotTouched = 0,
    Miss = 1,
    Hit = 2
}

public class GameControlerTools : MonoBehaviour {

    public ScreenTouchTypes ScreenTouched(GameMode_1 gameMode_1)
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
                        gameMode_1.CurrentLevel.RegisterHit(gameMode_1.HitsToWin, Time.time);
                        hit.collider.gameObject.SetActive(false);
                        return ScreenTouchTypes.Hit;
                    }
                    else if (touch.phase == TouchPhase.Began)
                        return ScreenTouchTypes.Miss; 
                }
            }
        }
        return ScreenTouchTypes.NotTouched ;
    }
}
