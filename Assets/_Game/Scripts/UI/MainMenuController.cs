using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wire PlayButton
        var playBtn = GameObject.Find("PlayButton")?.GetComponent<Button>();
        if (playBtn != null)
            playBtn.onClick.AddListener(OnNewGameClicked);
        else
            Debug.LogWarning("MainMenuController: PlayButton not found");

        // Wire ExitButton
        var exitBtn = GameObject.Find("ExitButton")?.GetComponent<Button>();
        if (exitBtn != null)
            exitBtn.onClick.AddListener(OnQuitClicked);
        else
            Debug.LogWarning("MainMenuController: ExitButton not found");
    }

    public void OnNewGameClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
