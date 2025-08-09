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

        UpdateButtonStates();
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
            if (dropUpPanel.activeSelf)
            {
                UpdateButtonStates();
            }
        }
    }

    private void Attack()
    {
        if (GameManager.instance.enemyTeam.Count > 0 && GameManager.instance.activePlayer != null)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            float distance = Vector3.Distance(GameManager.instance.activePlayer.transform.position, enemy.transform.position);
            if (distance <= 2f)
            {
                int damage = Random.Range(10, 21);
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy {enemy.name} takes {damage} damage. HP left: {enemy.hitPoints}");
                CompletePlayerAction();
            }
            else
            {
                Debug.Log("Enemy is too far to attack.");
            }
        }
    }

    private void Magic()
    {
        bool show = !fireButton.gameObject.activeSelf;
        fireButton.gameObject.SetActive(show);
        rainButton.gameObject.SetActive(show);
        if (show)
        {
            UpdateButtonStates();
        }
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
        if (GameManager.instance.enemyTeam.Count > 0 && GameManager.instance.activePlayer != null)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            float distance = Vector3.Distance(GameManager.instance.activePlayer.transform.position, enemy.transform.position);
            if (distance <= 4f)
            {
                int damage = Random.Range(15, 31);
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy {enemy.name} takes {damage} fire damage. HP left: {enemy.hitPoints}");
                fireButton.gameObject.SetActive(false);
                rainButton.gameObject.SetActive(false);
                CompletePlayerAction();
            }
            else
            {
                Debug.Log("Enemy is too far to cast Fire.");
            }
        }
    }

    private void CastRain()
    {
        if (GameManager.instance.enemyTeam.Count > 0 && GameManager.instance.activePlayer != null)
        {
            CharacterController enemy = GameManager.instance.enemyTeam[0];
            float distance = Vector3.Distance(GameManager.instance.activePlayer.transform.position, enemy.transform.position);
            if (distance <= 4f)
            {
                int damage = Random.Range(12, 36);
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy {enemy.name} takes {damage} rain damage. HP left: {enemy.hitPoints}");
                fireButton.gameObject.SetActive(false);
                rainButton.gameObject.SetActive(false);
                CompletePlayerAction();
            }
            else
            {
                Debug.Log("Enemy is too far to cast Rain.");
            }
        }
    }

    private void CompletePlayerAction()
    {
        HideMenu();
        GameManager.instance.EndTurn();
    }

    private void UpdateButtonStates()
    {
        if (attackButton == null || fireButton == null || rainButton == null)
        {
            return;
        }

        bool playerExists = GameManager.instance != null && GameManager.instance.activePlayer != null;
        bool enemyExists = GameManager.instance != null && GameManager.instance.enemyTeam.Count > 0;

        if (!playerExists || !enemyExists)
        {
            attackButton.interactable = false;
            fireButton.interactable = false;
            rainButton.interactable = false;
            return;
        }

        CharacterController player = GameManager.instance.activePlayer;
        CharacterController enemy = GameManager.instance.enemyTeam[0];

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

        attackButton.interactable = distance <= 2f;
        bool magicInRange = distance <= 4f;
        fireButton.interactable = magicInRange;
        rainButton.interactable = magicInRange;
    }
}
