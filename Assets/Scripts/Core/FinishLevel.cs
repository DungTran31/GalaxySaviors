using DungTran31.Dialogues;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungTran31.Core
{
    public class FinishLevel : MonoBehaviour
    {
        private void Update()
        {
            if (!DialogueManager.Instance.DialogueIsPlaying
                && DialogueManager.Instance.DialogueCount == 0)
            {
                SceneController.Instance.NextLevel(1); // Hub Scene
            }    
        }
    }
}
