using NUnit.Framework;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allChars.AddRange(FindObjectsByType<CharacterController>(FindObjectsSortMode.None));

        foreach(CharacterController cc in allChars)
        {
            if(cc.isEnemy == false)
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
