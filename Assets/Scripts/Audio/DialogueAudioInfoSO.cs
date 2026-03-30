using UnityEngine;

namespace DungTran31.Audio
{
    [CreateAssetMenu(fileName = "DialogueAudioInfo", menuName = "ScriptableObjects/DialogueAudioInfoSO", order = 1)]
    public class DialogueAudioInfoSO : ScriptableObject
    {
        public AudioClip[] dialogueTypingSoundClips;
        public string id;
        [Range(-3, 3)]
        public float minPitch = 0.5f;
        [Range(-3, 3)]
        public float maxPitch = 3f;
        [Range(1, 5)]
        public int frequencyLevel = 2;
        public bool stopAudioSource;
    }
}
