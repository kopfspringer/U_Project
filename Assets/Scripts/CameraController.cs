using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;
    public float dragSpeed = 1f;
    private Vector3 moveTarget;
    private Vector3 dragOrigin;
    private bool isDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDrag();

        if (!isDragging && moveTarget != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-delta.x, 0f, -delta.y) * dragSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
            moveTarget = transform.position;
            dragOrigin = Input.mousePosition;
        }
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}
