using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryMenuController : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Try to find existing MainMenuButton first
        var existingBtn = GameObject.Find("MainMenuButton")?.GetComponent<Button>();
        if (existingBtn != null)
        {
            existingBtn.onClick.AddListener(OnMainMenuClicked);
            return;
        }

        // No UI exists, create it programmatically
        CreateVictoryUI();
    }

    void CreateVictoryUI()
    {
        // Create Canvas
        var canvasGO = new GameObject("VictoryCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create dark background
        var bgGO = new GameObject("Background");
        bgGO.transform.SetParent(canvasGO.transform, false);
        var bgImage = bgGO.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 1f);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // Create "VICTORY!" text
        var textGO = new GameObject("VictoryText");
        textGO.transform.SetParent(canvasGO.transform, false);
        var tmpText = textGO.AddComponent<TextMeshProUGUI>();
        tmpText.text = "VICTORY!";
        tmpText.fontSize = 72;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.green;
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.7f);
        textRect.anchorMax = new Vector2(0.5f, 0.7f);
        textRect.sizeDelta = new Vector2(600, 100);

        // Create "Main Menu" button
        var buttonGO = new GameObject("MainMenuButton");
        buttonGO.transform.SetParent(canvasGO.transform, false);
        var buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        var button = buttonGO.AddComponent<Button>();
        button.onClick.AddListener(OnMainMenuClicked);
        var buttonRect = buttonGO.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.4f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.4f);
        buttonRect.sizeDelta = new Vector2(200, 50);

        // Create button text
        var btnTextGO = new GameObject("Text");
        btnTextGO.transform.SetParent(buttonGO.transform, false);
        var btnText = btnTextGO.AddComponent<TextMeshProUGUI>();
        btnText.text = "Main Menu";
        btnText.fontSize = 24;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;
        var btnTextRect = btnTextGO.GetComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.sizeDelta = Vector2.zero;

        // Add EventSystem if not present
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
