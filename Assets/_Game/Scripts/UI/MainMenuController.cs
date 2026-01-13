using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
