using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    public static BattleMenu instance;

    [Header("Menu Elements")]
    public GameObject menuRoot;
    public Button attackButton;
    public Button magicButton;
    public Button itemsButton;
    public Button waitButton;

    private void Awake()
    {
        instance = this;

        if (menuRoot == null)
        {
            CreateMenu();
        }

        Hide();

        if (waitButton != null)
        {
            waitButton.onClick.AddListener(() =>
            {
                Hide();
                GameManager.instance.EndTurn();
            });
        }
    }

    private void CreateMenu()
    {
        Canvas canvas = new GameObject("BattleMenuCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();

        menuRoot = canvas.gameObject;

        GameObject panel = new GameObject("MenuPanel");
        panel.transform.SetParent(canvas.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(200, 300);
        VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 10f;

        attackButton = CreateButton(panel.transform, "Attack");
        magicButton = CreateButton(panel.transform, "Magic");
        itemsButton = CreateButton(panel.transform, "Items");
        waitButton = CreateButton(panel.transform, "Wait");
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
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
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

    public void Show()
    {
        if (menuRoot != null)
        {
            menuRoot.SetActive(true);
        }
    }

    public void Hide()
    {
        if (menuRoot != null)
        {
            menuRoot.SetActive(false);
        }
    }
}

