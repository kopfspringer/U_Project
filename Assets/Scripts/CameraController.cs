using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;
    private Vector3 moveTarget;
    public float dragSpeed = 0.1f;
    private Vector3 lastMousePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseDrag();

        if (moveTarget != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            moveTarget = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, 0f, -delta.y) * dragSpeed;
            transform.position += move;

            lastMousePosition = Input.mousePosition;
            moveTarget = transform.position;
        }
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}
