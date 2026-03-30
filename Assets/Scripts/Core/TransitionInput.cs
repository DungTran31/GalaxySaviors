using DungTran31.Dialogues;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungTran31.Core
{
    public class TransitionInput : MonoBehaviour
    {
        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                && !DialogueManager.Instance.DialogueIsPlaying 
                && DialogueManager.Instance.DialogueCount <= 0)
            {
                SceneController.Instance.NextLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
