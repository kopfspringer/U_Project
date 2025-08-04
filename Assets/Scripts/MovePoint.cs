using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.MoveActivePlayerToPoint(transform.position);
    }
}
