using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveGrid : MonoBehaviour
{

    public MovePoint startPoint;

    public Vector2Int spawnRange;

    public LayerMask whatIsGround;

    public LayerMask whatIsObstacle;

    public float obstacleCheckRange;

    public List<MovePoint> allMovePoints = new List<MovePoint>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Battle_1")
        {
            GenerateMoveGrid();
        }

        if(sceneName == "IntroScene")
        {
            OpenMenu.instance.HideMenus();
        }
        //HideMovePoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMoveGrid()
    {
        for (int x = -spawnRange.x; x <= spawnRange.x; x++)
        {
            for (int y = -spawnRange.y; y <= spawnRange.y; y++)
            {
                // Prüfe ob Untergrund existiert, wenn ja mache MovePoint
                RaycastHit hit;

                if (Physics.Raycast(transform.position + new Vector3(x, 10f, y), Vector3.down, out hit, 20f, whatIsGround))
                {
                    if (Physics.OverlapSphere(hit.point, obstacleCheckRange, whatIsObstacle).Length == 0)
                    {
                        MovePoint newPoint = Instantiate(startPoint, hit.point, transform.rotation);
                        newPoint.transform.SetParent(transform); //MovePoints childs of movementGrid

                        allMovePoints.Add(newPoint);
                    }
                }
            }
        }
        startPoint.gameObject.SetActive(false);
    }

    public void HideMovePoints()
    {
        /*
        for (int i = 0; i < allMovePoints.Count; i++)
        {
            allMovePoints[i].gameObject.SetActive(false);
        }
        */
        foreach (MovePoint movePoint in allMovePoints)
        {
            movePoint.gameObject.SetActive(false);
        }
    }
}
