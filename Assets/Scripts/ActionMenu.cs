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

    private enum PendingAttack { None, Physical, Fire, Rain }
    private PendingAttack pendingAttack = PendingAttack.None;
    private int pendingRange;

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
        if (GameManager.instance.activePlayer != null)
        {
            pendingAttack = PendingAttack.Physical;
            pendingRange = 2;
            MoveGrid.instance.ShowAttackRange(GameManager.instance.activePlayer.transform.position, pendingRange);
            if (dropUpPanel != null)
            {
                dropUpPanel.SetActive(false);
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
        if (GameManager.instance.activePlayer != null)
        {
            pendingAttack = PendingAttack.Fire;
            pendingRange = 4;
            MoveGrid.instance.ShowAttackRange(GameManager.instance.activePlayer.transform.position, pendingRange);
            fireButton.gameObject.SetActive(false);
            rainButton.gameObject.SetActive(false);
            if (dropUpPanel != null)
            {
                dropUpPanel.SetActive(false);
            }
        }
    }

    private void CastRain()
    {
        if (GameManager.instance.activePlayer != null)
        {
            pendingAttack = PendingAttack.Rain;
            pendingRange = 4;
            MoveGrid.instance.ShowAttackRange(GameManager.instance.activePlayer.transform.position, pendingRange);
            fireButton.gameObject.SetActive(false);
            rainButton.gameObject.SetActive(false);
            if (dropUpPanel != null)
            {
                dropUpPanel.SetActive(false);
            }
        }
    }

    private void CompletePlayerAction()
    {
        HideMenu();
        GameManager.instance.EndTurn();
    }

    public bool TryExecuteAttackOn(CharacterController target)
    {
        if (pendingAttack == PendingAttack.None || GameManager.instance.activePlayer == null)
        {
            return false;
        }

        CharacterController player = GameManager.instance.activePlayer;
        float distance = Vector3.Distance(player.transform.position, target.transform.position);
        if (distance > pendingRange || !target.isEnemy)
        {
            MoveGrid.instance.HideMovePoints();
            pendingAttack = PendingAttack.None;
            return false;
        }

        int damage = 0;
        switch (pendingAttack)
        {
            case PendingAttack.Physical:
                damage = Random.Range(10, 21);
                Debug.Log($"Enemy {target.name} takes {damage} damage. HP left: {target.hitPoints - damage}");
                break;
            case PendingAttack.Fire:
                damage = Random.Range(15, 31);
                Debug.Log($"Enemy {target.name} takes {damage} fire damage. HP left: {target.hitPoints - damage}");
                break;
            case PendingAttack.Rain:
                damage = Random.Range(12, 36);
                Debug.Log($"Enemy {target.name} takes {damage} rain damage. HP left: {target.hitPoints - damage}");
                break;
        }

        target.TakeDamage(damage);
        MoveGrid.instance.HideMovePoints();
        pendingAttack = PendingAttack.None;
        CompletePlayerAction();
        return true;
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

        // Always allow the action buttons to be interacted with if both
        // a player and an enemy exist.  The actual range validation is
        // handled when the player attempts to execute the attack in
        // TryExecuteAttackOn.  Previously these buttons were disabled when
        // the enemy was out of range, which prevented the player from
        // selecting an attack at all.  This change keeps them enabled so the
        // player can choose an attack even if the enemy is far away.
        attackButton.interactable = true;
        fireButton.interactable = true;
        rainButton.interactable = true;
    }
}
