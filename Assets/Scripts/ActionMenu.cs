using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public static ActionMenu instance;

    [Header("UI References")]
    public Button actionButton;
    public GameObject dropUpPanel;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button magicButton;
    [SerializeField] private Button restButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        HideMenu();

        if (actionButton != null)
        {
            actionButton.onClick.AddListener(ToggleDropUp);
        }

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(Attack);
        }
        if (magicButton != null)
        {
            magicButton.onClick.AddListener(Magic);
        }
        if (restButton != null)
        {
            restButton.onClick.AddListener(Rest);
        }
    }

    public void ShowMenu()
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(true);
        }

        if (dropUpPanel != null)
        {
            dropUpPanel.SetActive(false);
        }
    }

    public void HideMenu()
    {
        if (actionButton != null)
        {
            actionButton.gameObject.SetActive(false);
        }

        if (dropUpPanel != null)
        {
            dropUpPanel.SetActive(false);
        }
    }

    private void ToggleDropUp()
    {
        if (dropUpPanel != null)
        {
            dropUpPanel.SetActive(!dropUpPanel.activeSelf);
        }
    }

    private void Attack()
    {
        if (GameManager.instance.enemyTeam.Count > 0)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            int damage = Random.Range(10, 21);
            enemy.TakeDamage(damage);
            Debug.Log($"Enemy {enemy.name} takes {damage} damage. HP left: {enemy.hitPoints}");
        }

        CompletePlayerAction();
    }

    private void Magic()
    {
        CompletePlayerAction();
    }

    private void Rest()
    {
        CompletePlayerAction();
    }

    private void CompletePlayerAction()
    {
        if (dropUpPanel != null)
        {
            dropUpPanel.SetActive(false);
        }

        HideMenu();
        GameManager.instance.EndTurn();
    }
}
