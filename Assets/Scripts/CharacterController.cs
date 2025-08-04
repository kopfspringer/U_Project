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
        if (isEnemy && Vector3.Distance(transform.position, moveTarget) < 0.01f)
        {
            MoveTowardsPlayer();
        }

        //moving to a point
        if (transform.position != moveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.instance.activePlayer == this)
            {
                CameraController.instance.SetMoveTarget(transform.position);
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (GameManager.instance.playerTeam.Count == 0)
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.playerTeam[0].transform.position;
        Vector3 diff = playerPos - transform.position;
        Vector3 step = Vector3.zero;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
        {
            step = new Vector3(Mathf.Sign(diff.x), 0f, 0f);
        }
        else if (diff.z != 0f)
        {
            step = new Vector3(0f, 0f, Mathf.Sign(diff.z));
        }

        Vector3 newTarget = transform.position + step;

        if (newTarget == playerPos)
        {
            if (step.x != 0f && diff.z != 0f)
            {
                step = new Vector3(0f, 0f, Mathf.Sign(diff.z));
                newTarget = transform.position + step;
            }
            else if (step.z != 0f && diff.x != 0f)
            {
                step = new Vector3(Mathf.Sign(diff.x), 0f, 0f);
                newTarget = transform.position + step;
            }
        }

        if (newTarget != playerPos && step != Vector3.zero)
        {
            moveTarget = newTarget;
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        moveTarget = pointToMoveTo;
    }
}
