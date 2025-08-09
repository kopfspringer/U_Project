using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveGrid : MonoBehaviour
{
    public static MoveGrid instance;

    public MovePoint startPoint;
    public Vector2Int spawnRange;
    public LayerMask whatIsGround;
    public LayerMask whatIsObstacle;
    public float obstacleCheckRange;

    public List<MovePoint> allMovePoints = new List<MovePoint>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Battle_1")
        {
            GenerateMoveGrid();
        }

        if (sceneName == "IntroScene")
        {
            OpenMenu.instance.HideMenus();
        }
        HideMovePoints();
    }

    void Update()
    {
    }

    public void GenerateMoveGrid()
    {
        for (int x = -spawnRange.x; x <= spawnRange.x; x++)
        {
            for (int y = -spawnRange.y; y <= spawnRange.y; y++)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + new Vector3(x, 10f, y), Vector3.down, out hit, 20f, whatIsGround))
                {
                    if (Physics.OverlapSphere(hit.point, obstacleCheckRange, whatIsObstacle).Length == 0)
                    {
                        MovePoint newPoint = Instantiate(startPoint, hit.point, transform.rotation);
                        newPoint.transform.SetParent(transform);

                        allMovePoints.Add(newPoint);
                    }
                }
            }
        }
        startPoint.gameObject.SetActive(false);
    }

    public void HideMovePoints()
    {
        foreach (MovePoint movePoint in allMovePoints)
        {
            movePoint.gameObject.SetActive(false);
            movePoint.ResetColor();
        }
    }

    public List<MovePoint> GetPointsInRange(Vector3 center, int range)
    {
        List<MovePoint> pointsInRange = new List<MovePoint>();

        foreach (MovePoint movePoint in allMovePoints)
        {
            if (Vector3.Distance(center, movePoint.transform.position) <= range)
            {
                pointsInRange.Add(movePoint);
            }
        }

        return pointsInRange;
    }

    public void ShowMovePointsAround(Vector3 center, int range)
    {
        HideMovePoints();

        List<MovePoint> pointsToShow = GetPointsInRange(center, range);

        foreach (MovePoint movePoint in pointsToShow)
        {
            movePoint.gameObject.SetActive(true);
        }
    }

    public void ShowAttackRange(Vector3 center, int range)
    {
        HideMovePoints();

        List<MovePoint> pointsToShow = GetPointsInRange(center, range);

        foreach (MovePoint movePoint in pointsToShow)
        {
            movePoint.gameObject.SetActive(true);
            movePoint.SetColor(Color.red);
        }
    }
}

