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
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.instance.activePlayer == this)
            {
                CameraController.instance.SetMoveTarget(transform.position);
            }
        }
        else if (isMoving)
        {
            isMoving = false;
            GameManager.instance.CharacterFinishedMove(this);
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        moveTarget = pointToMoveTo;
        isMoving = true;
    }

    private void OnMouseDown()
    {
        if (!isEnemy)
        {
            GameManager.instance.SelectCharacter(this);
        }
    }
}
