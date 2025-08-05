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

    private Button fireButton;
    private Button rainButton;

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
        if (fireButton == null || rainButton == null)
        {
            CreateMagicButtons();
        }

        bool show = !fireButton.gameObject.activeSelf;
        fireButton.gameObject.SetActive(show);
        rainButton.gameObject.SetActive(show);
    }

    private void Rest()
    {
        CompletePlayerAction();
    }

    private void CastFire()
    {
        CompletePlayerAction();
    }

    private void CastRain()
    {
        CompletePlayerAction();
    }

    private void CreateMagicButtons()
    {
        fireButton = CreateButton(dropUpPanel.transform, "Fire");
        rainButton = CreateButton(dropUpPanel.transform, "Rain");

        fireButton.gameObject.SetActive(false);
        rainButton.gameObject.SetActive(false);

        fireButton.onClick.AddListener(CastFire);
        rainButton.onClick.AddListener(CastRain);
    }

    private Button CreateButton(Transform parent, string text)
    {
        GameObject buttonObj = new GameObject(text + "Button");
        buttonObj.transform.SetParent(parent, false);

        Image image = buttonObj.AddComponent<Image>();
        image.color = Color.white;

        Button button = buttonObj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        Text txt = textObj.AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.text = text;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.black;

        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 40);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return button;
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
