using DungTran31.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungTran31.Core
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject helpText;
        [SerializeField] private GameObject OText;
        [SerializeField] private GameObject options;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private GameObject[] cubes;

        private void Awake()
        {
            SetActiveAllCubes(true);
            settingPanel.SetActive(false);
            aboutPanel.SetActive(false);
            helpText.SetActive(true);
            OText.SetActive(true);
            options.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                ShowOption();
            }
        }

        public void EnterHub()
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.pressed);
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }

        public void ShowSetting()
        {
            TogglePanel(settingPanel, false, true);
        }

        public void HideSetting()
        {
            TogglePanel(settingPanel, true, true);
        }

        public void ShowAbout()
        {
            TogglePanel(aboutPanel, false, true);
        }

        public void HideAbout()
        {
            TogglePanel(aboutPanel, true, true);
        }


        public void OpenLink(string link)
        {
            Application.OpenURL(link);
        }

        private void ShowOption()
        {
            helpText.SetActive(false);
            options.SetActive(true);
            DissolveMainMenu.Instance.SetAppear();
        }

        public void Quit()
        {
            Application.Quit(); // Quits the game (only works in build)

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Exits play mode
#endif
        }

        private void SetActiveAllCubes(bool active)
        {
            foreach (var cube in cubes)
            {
                cube.SetActive(active);
            }
        }
        private void TogglePanel(GameObject panelToShow, bool showCubes, bool pauseMusic)
        {
            SetActiveAllCubes(showCubes);
            panelToShow.SetActive(!showCubes);
            OText.SetActive(showCubes);

            if (pauseMusic)
            {
                if (showCubes)
                {
                    AudioManager.Instance.musicSource.UnPause();
                }
                else
                {
                    AudioManager.Instance.musicSource.Pause();
                }
            }

            AudioManager.Instance.PlaySfx(AudioManager.Instance.pressed);
        }

    }
}
