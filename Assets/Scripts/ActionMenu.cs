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

    [SerializeField] private Button fireButton;
    [SerializeField] private Button rainButton;

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

        if (fireButton != null)
        {
            fireButton.onClick.AddListener(CastFire);
            fireButton.gameObject.SetActive(false);
        }
        if (rainButton != null)
        {
            rainButton.onClick.AddListener(CastRain);
            rainButton.gameObject.SetActive(false);
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
        bool show = !fireButton.gameObject.activeSelf;
        fireButton.gameObject.SetActive(show);
        rainButton.gameObject.SetActive(show);
    }

    private void Rest()
    {
        CharacterController player = GameManager.instance.activePlayer;
        if (player != null && !player.isEnemy)
        {
            int healAmount = 30;
            player.Heal(healAmount);
            Debug.Log($"Player {player.name} rests and recovers {healAmount} HP. HP now: {player.hitPoints}");
        }

        CompletePlayerAction();
    }

    private void CastFire()
    {
        if (GameManager.instance.enemyTeam.Count > 0)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            int damage = Random.Range(15, 31);
            enemy.TakeDamage(damage);
            Debug.Log($"Enemy {enemy.name} takes {damage} fire damage. HP left: {enemy.hitPoints}");
        }

        fireButton.gameObject.SetActive(false);
        rainButton.gameObject.SetActive(false);

        CompletePlayerAction();
    }

    private void CastRain()
    {
        if (GameManager.instance.enemyTeam.Count > 0)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            int damage = Random.Range(12, 36);
            enemy.TakeDamage(damage);
            Debug.Log($"Enemy {enemy.name} takes {damage} rain damage. HP left: {enemy.hitPoints}");
        }

        fireButton.gameObject.SetActive(false);
        rainButton.gameObject.SetActive(false);

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
