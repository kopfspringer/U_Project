using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private Renderer rend;
    private Color defaultColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            defaultColor = rend.material.color;
        }
    }

    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    public void ResetColor()
    {
        SetColor(defaultColor);
    }

    private void OnMouseDown()
    {
        GameManager.instance.MoveActivePlayerToPoint(transform.position);
    }
}
