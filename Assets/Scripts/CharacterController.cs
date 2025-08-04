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

        if (hpText != null)
        {
            hpText.text = hitPoints.ToString();
            if (Camera.main != null)
            {
                hpText.transform.LookAt(Camera.main.transform);
                hpText.transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    public void MoveToPoint(Vector3 pointToMoveTo)
    {
        moveTarget = pointToMoveTo;
    }
}
