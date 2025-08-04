using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    public NavMeshAgent navAgent;
    private bool isMoving;

    public bool isEnemy;

    public int hitPoints = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

        //moving to a point
        if (transform.position != moveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.instance.activePlayer == this)
            {
                CameraController.instance.SetMoveTarget(transform.position);
            }
        }
        else if (isMoving)
        {
            isMoving = false;

            if (GameManager.instance.activePlayer == this)
            {
                ActionMenu.instance.ShowMenu();
            }
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        moveTarget = pointToMoveTo;
        isMoving = true;
        ActionMenu.instance.HideMenu();
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;

        if (hitPoints < 0)
        {
            hitPoints = 0;
        }
    }
}
