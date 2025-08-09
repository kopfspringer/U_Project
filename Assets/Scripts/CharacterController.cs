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
    public Vector3 hpOffset = new Vector3(0f, 2f, 0f);
    private bool playerMovePending;
    private int lastTurnProcessed;
    private bool isDead;

    private Transform hpBarParent;
    private Transform hpBarFill;
    private float maxHpBarWidth = 1.5f;
    private float hpBarHeight = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;

        GameObject hpParentObj = new GameObject("HPBar");
        hpParentObj.transform.SetParent(transform);
        hpParentObj.transform.localPosition = hpOffset;
        hpBarParent = hpParentObj.transform;

        GameObject hpFillObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hpFillObj.name = "HPBarFill";
        hpFillObj.transform.SetParent(hpBarParent);
        hpFillObj.transform.localPosition = Vector3.zero;
        hpFillObj.transform.localScale = new Vector3(maxHpBarWidth, hpBarHeight, 0.1f);
        hpFillObj.GetComponent<Renderer>().material.color = Color.red;
        Destroy(hpFillObj.GetComponent<Collider>());
        hpBarFill = hpFillObj.transform;
        UpdateHPBar();
        if (isEnemy)
        {
            lastTurnProcessed = GameManager.instance.turnCounter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isEnemy && Vector3.Distance(transform.position, moveTarget) < 0.01f && lastTurnProcessed < GameManager.instance.turnCounter)
        {
            MoveTowardsPlayer();
            lastTurnProcessed = GameManager.instance.turnCounter;
        }

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

        if (!isEnemy && playerMovePending && Vector3.Distance(transform.position, moveTarget) < 0.01f)
        {
            playerMovePending = false;
            isMoving = false;

            if (GameManager.instance.activePlayer == this)
            {
                ActionMenu.instance.ShowMenu();
            }
        }

        if (hpBarParent != null && Camera.main != null)
        {
            hpBarParent.LookAt(Camera.main.transform);
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
        if (isDead)
        {
            return;
        }

        moveTarget = pointToMoveTo;
        if (!isEnemy)
        {
            playerMovePending = true;
            isMoving = true;
        }
    }

    private void OnMouseDown()
    {
        if (isDead)
        {
            return;
        }
        if (ActionMenu.instance != null && ActionMenu.instance.TryExecuteAttackOn(this))
        {
            return;
        }
        if (!isEnemy)
        {
            GameManager.instance.SelectCharacter(this);
            isMoving = true;
            ActionMenu.instance.HideMenu();
        }
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }
        UpdateHPBar();
        if (hitPoints == 0 && !isDead)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        hitPoints += amount;
        if (hitPoints > 100)
        {
            hitPoints = 100;
        }
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (hpBarFill != null)
        {
            float width = maxHpBarWidth * hitPoints / 100f;
            hpBarFill.localScale = new Vector3(width, hpBarHeight, 0.1f);
            hpBarFill.localPosition = new Vector3((width - maxHpBarWidth) / 2f, 0f, 0f);
        }
    }

    private void Die()
    {
        isDead = true;
        moveTarget = transform.position;
        playerMovePending = false;
        if (hpBarParent != null)
        {
            hpBarParent.gameObject.SetActive(false);
        }
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
