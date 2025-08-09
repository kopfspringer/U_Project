using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private Renderer rend;
    private Color defaultColor;
    private Collider col;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

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

    public void SetClickable(bool clickable)
    {
        if (col != null)
        {
            col.enabled = clickable;
        }
    }

    private void OnMouseDown()
    {
        GameManager.instance.MoveActivePlayerToPoint(transform.position);
    }
}
