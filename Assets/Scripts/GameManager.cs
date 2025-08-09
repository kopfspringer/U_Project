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

        // Allow action menu to appear only after the player finishes a move.
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
        ActionMenu.instance.HideMenu();

        bool playerFinishedTurn = !activePlayer.isEnemy;

        if (playerFinishedTurn)
        {
            turnCounter++;
        }

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
        // The action menu will be displayed once the new active player finishes moving.
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        if (playerTeam.Count > 0)
        {
            CharacterController player = playerTeam[0];
            CharacterController enemy = activePlayer;
            if (enemy != null)
            {
                float distance = Vector3.Distance(enemy.transform.position, player.transform.position);
                if (distance <= 2f)
                {
                    int damage = Random.Range(10, 21);
                    player.TakeDamage(damage);
                    Debug.Log($"Player {player.name} takes {damage} damage. HP left: {player.hitPoints}");
                }
                else
                {
                    Debug.Log("Player is too far away to be attacked.");
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        EndTurn();
    }
}
