using UnityEngine;
using DungTran31.Core;
using DungTran31.GamePlay.Enemy;
using DungTran31.UI;

namespace DungTran31.Dialogues
{
    public class DialogueTriggerLevel : MonoBehaviour
    {
        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJSON;

        private void OnEnable() => CinematicBars.OnCinematicBarsFinished += PlayDialogue;
    

        private void OnDisable() => CinematicBars.OnCinematicBarsFinished -= PlayDialogue;

        private void PlayDialogue()
        {
            if (!DialogueManager.Instance.DialogueIsPlaying)
            {
                DialogueManager.Instance.EnterDialogueMode(inkJSON);
            }
        }
    }
}
