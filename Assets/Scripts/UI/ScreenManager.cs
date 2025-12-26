using System.Collections;
using UnityEngine;

namespace TopDown.UI
{
    public class ScreenManager : MonoBehaviour
    {
        [Header("Main Menu Screens")]
        [SerializeField] private GameObject mainMenuPanel = null;
        [SerializeField] private GameObject settingPanel = null;

        void Start()
        {
            ShowMainMenu();
        }

        void DeactivateAllScreens()
        {
            if (mainMenuPanel) mainMenuPanel.SetActive(false);
            if (settingPanel) settingPanel.SetActive(false);
        }

        public void ShowMainMenu()
        {
            ShowScreen(mainMenuPanel);
        }

        public void ShowSettings()
        {
            ShowScreen(settingPanel);
        }

        public void ShowScreen(GameObject target)
        {
            if (!target) return;
            StartCoroutine(SwitchScreen(target));
        }

        IEnumerator SwitchScreen(GameObject next)
        {
            DeactivateAllScreens();
            next.SetActive(true);
            yield break;
        }
    }
}
