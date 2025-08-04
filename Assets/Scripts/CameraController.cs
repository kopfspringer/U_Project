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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTarget != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}
