﻿using UnityEngine;

public class GameControlerScript : MonoBehaviour {

    public GameObject GameObjectFactory, GameBoard;
    public int PointsBoardGranurality = 10, PointSpawnTimeRange = 10;
    PointsBoard PointsBoard;

    private void Start()
    {
        PointsBoard = new PointsBoard(GameBoard.transform.localScale.x
            , GameBoard.transform.localScale.y
            , PointsBoardGranurality
            , PointSpawnTimeRange
            , GameObjectFactory);
    }


    public void FixedUpdate()
    {
        HandleTouch();
    }

    public void ActivateOneOfPoints()
    {
        StartCoroutine(PointsBoard.ActivateOneOfPoints());
    }

    public void HandleTouch()
    {
        int nbTouches = Input.touchCount;
        if (nbTouches > 0)
        {
            for (int i = 0; i < nbTouches; i++)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray screenRay = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(screenRay, out hit))
                    {
                        hit.collider.gameObject.SetActive(false);
                        ActivateOneOfPoints();
                    }
                }
            }
        }
    }
}
