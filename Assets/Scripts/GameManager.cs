using UnityEngine;
using UnityEngine.SceneManagement;
using TopDown.UI;

public class GameManager : MonoBehaviour
{
    private ScreenManager screenManager;

    private void Awake()
    {
        screenManager = GetComponent<ScreenManager>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        if (screenManager != null)
        {
            screenManager.ShowSettings();
        }
    }

    public void BackToMainMenu()
    {
        if (screenManager != null)
        {
            screenManager.ShowMainMenu();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
