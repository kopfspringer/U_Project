using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public CharacterController activePlayer;

    public List<CharacterController> allChars = new List<CharacterController>();
    public List<CharacterController> playerTeam = new List<CharacterController>();
    public List<CharacterController> enemyTeam = new List<CharacterController>();

    public int turnCounter = 0;
    private int currentCharIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allChars.AddRange(FindObjectsByType<CharacterController>(FindObjectsSortMode.None));

        foreach (CharacterController cc in allChars)
        {
            if (cc.isEnemy == false)
            {
                playerTeam.Add(cc);
            }
            else
            {
                enemyTeam.Add(cc);
            }
        }

        allChars.Clear();

        allChars.AddRange(playerTeam);
        allChars.AddRange(enemyTeam);

        currentCharIndex = 0;
        if (allChars.Count > 0)
        {
            activePlayer = allChars[currentCharIndex];
            CameraController.instance.SetMoveTarget(activePlayer.transform.position);

            if (activePlayer.isEnemy)
            {
                StartCoroutine(EnemyTurn());
            }
        }
        if (activePlayer == null && playerTeam.Count > 0)
        {
            activePlayer = playerTeam[0];
        }

        if (activePlayer != null)
        {
            ActionMenu.instance.ShowMenu();
        }

    }

    public void SelectCharacter(CharacterController cc)
    {
        if (cc == activePlayer && !cc.isEnemy)
        {
            MoveGrid.instance.ShowMovePointsAround(cc.transform.position, 5);
        }
    }

    public void MoveActivePlayerToPoint(Vector3 point)
    {
        if (activePlayer != null && !activePlayer.isEnemy)
        {
            activePlayer.MoveToPoint(point);
            MoveGrid.instance.HideMovePoints();
        }
    }

    public void CharacterFinishedMove(CharacterController cc)
    {
        if (cc == activePlayer)
        {
            if (!cc.isEnemy)
            {
                if (BattleMenu.instance != null)
                {
                    BattleMenu.instance?.Show();
                }
                else
                {
                    EndTurn();
                }
               
            }
            else
            {
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {
        BattleMenu.instance?.Hide();

        currentCharIndex++;
        if (currentCharIndex >= allChars.Count)
        {
            currentCharIndex = 0;
        }

        activePlayer = allChars[currentCharIndex];

        CameraController.instance.SetMoveTarget(activePlayer.transform.position);

        if (activePlayer.isEnemy)
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        List<MovePoint> possibleMoves = MoveGrid.instance.GetPointsInRange(activePlayer.transform.position, 5);

        if (possibleMoves.Count > 0)
        {
            int index = Random.Range(0, possibleMoves.Count);
            activePlayer.MoveToPoint(possibleMoves[index].transform.position);
        }
    }
    public void OnPlayerMoveComplete()
    {
        turnCounter++;
    }
}
