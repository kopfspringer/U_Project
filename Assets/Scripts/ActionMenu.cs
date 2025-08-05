using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionMenu : MonoBehaviour
{
    public static ActionMenu instance;

    [Header("UI References")]
    public Button actionButton;
    public GameObject dropUpPanel;
    public Button buttonPrefab;

    private Button attackButton;
    private Button magicButton;
    private Button restButton;

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

        if (dropUpPanel != null && buttonPrefab != null)
        {
            attackButton = CreateMenuButton("Attack", Attack, 0);
            magicButton = CreateMenuButton("Magic", Magic, 1);
            restButton = CreateMenuButton("Rest", Rest, 2);
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

    private Button CreateMenuButton(string label, UnityEngine.Events.UnityAction action, int index)
    {
        Button btn = Instantiate(buttonPrefab, dropUpPanel.transform);
        btn.onClick.AddListener(action);

        TMP_Text text = btn.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = label;
        }

        RectTransform rect = btn.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(0, -index * rect.sizeDelta.y);
        }

        return btn;
    }
}
