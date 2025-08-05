using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    public NavMeshAgent navAgent;
    private bool isMoving;

    public bool isEnemy;

    public int hitPoints = 100;
    private TextMeshPro hpText;
    public Vector3 hpOffset = new Vector3(0f, 2f, 0f);
    private bool playerMovePending;
    private int lastTurnProcessed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;

        GameObject hpObj = new GameObject("HPDisplay");
        hpObj.transform.SetParent(transform);
        hpObj.transform.localPosition = hpOffset;
        hpText = hpObj.AddComponent<TextMeshPro>();
        hpText.alignment = TextAlignmentOptions.Center;
        hpText.fontSize = 3f;
        hpText.color = Color.red;

        // Assign a valid font asset so the HP value renders instead of the TMP placeholder
        TMP_FontAsset fontAsset = TMP_Settings.defaultFontAsset;
        if (fontAsset == null)
        {
            fontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        }
        if (fontAsset != null)
        {
            hpText.font = fontAsset;
        }

        hpText.text = hitPoints.ToString();
        if (isEnemy)
        {
            lastTurnProcessed = GameManager.instance.turnCounter;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            GameManager.instance.OnPlayerMoveComplete();
            playerMovePending = false;
            isMoving = false;

            if (GameManager.instance.activePlayer == this)
            {
                ActionMenu.instance.ShowMenu();
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

            if (hpText != null)
            {
                hpText.text = hitPoints.ToString();
                if (Camera.main != null)
                {
                    hpText.transform.LookAt(Camera.main.transform);
                }
            }
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        moveTarget = pointToMoveTo;
        if (!isEnemy)
        {
            playerMovePending = true;
            isMoving = true;
        }
    }

    private void OnMouseDown()
    {
        if (!isEnemy)
        {
            GameManager.instance.SelectCharacter(this);
            isMoving = true;
            ActionMenu.instance.HideMenu();
        }
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

        if (hpText != null)
        {
            hpText.text = hitPoints.ToString();
            if (Camera.main != null)
            {
                hpText.transform.LookAt(Camera.main.transform);
            }
        }
    }
}
