using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        //FindFirstObjectByType<CharacterController>().MoveToPoint(transform.position);
        GameManager.instance.activePlayer.MoveToPoint(transform.position);
    }
}
