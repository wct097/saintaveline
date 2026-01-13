using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wire PlayButton (Retry)
        var retryBtn = GameObject.Find("PlayButton")?.GetComponent<Button>();
        if (retryBtn != null)
            retryBtn.onClick.AddListener(OnRetryClicked);
        else
            Debug.LogWarning("GameOverMenuController: PlayButton (Retry) not found");

        // Wire MainMenuButton (if exists)
        var mainMenuBtn = GameObject.Find("MainMenuButton")?.GetComponent<Button>();
        if (mainMenuBtn != null)
            mainMenuBtn.onClick.AddListener(OnMainMenuClicked);
    }

    public void OnRetryClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
