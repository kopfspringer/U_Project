using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public float moveSpeed;
    public float dragSpeed;
    private Vector3 moveTarget;
    private Vector3 lastMousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            moveTarget = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
            Vector3 move = new Vector3(-delta.x, 0f, -delta.y) * dragSpeed * Time.deltaTime;
            transform.position += move;
            moveTarget = transform.position;
        }
        else if (moveTarget != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}
