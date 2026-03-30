using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using DungTran31.Utilities;
using DungTran31.UI;

namespace DungTran31.Core
{
    public class FinalManager : MonoBehaviour
    {
        public void MainMenu()
        {
            SceneController.Instance.NextLevel(0);
        }

        public void Quit()
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.pressed);
            Application.Quit(); // Quits the game (only works in build)

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Exits play mode
#endif
        }


    }
}
