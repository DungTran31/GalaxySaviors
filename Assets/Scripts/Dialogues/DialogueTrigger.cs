using DungTran31.Core;
using UnityEngine;

namespace DungTran31.Dialogues
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJSON;

        private void Update()
        {
            if (!DialogueManager.Instance.DialogueIsPlaying)
            {
                // Check if the dialogue has not started and the interact button is pressed
                if (InputManager.Instance.GetSubmitPressed() && DialogueManager.Instance.DialogueCount > 0)
                {
                    DialogueManager.Instance.EnterDialogueMode(inkJSON);
                } 
            }
        }
    }
}
 