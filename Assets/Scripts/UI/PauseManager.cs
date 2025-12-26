using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TopDown.Audio;

namespace TopDown.UI
{
    public class PauseManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;
        
        private bool isPaused = false;

        private void Start()
        {
            // Hide all panels at start
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Pause()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
            Time.timeScale = 0f;
            isPaused = true;
            
            // Pause background music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PauseMusic();
            }
        }

        public void Resume()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;
            
            // Resume background music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ResumeMusic();
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void BackToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OpenSettings()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }

        public void CloseSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
            if (pausePanel != null && isPaused)
            {
                pausePanel.SetActive(true);
            }
        }
    }
}
