using UnityEngine;
using DungTran31.Utilities;
using UnityEngine.SceneManagement;

namespace DungTran31.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Source")]
        [SerializeField] internal AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clip")]
        public AudioClip background;
        public AudioClip bossBackground;
        public AudioClip death;
        public AudioClip enemyDeath;
        public AudioClip shoot;
        public AudioClip dash;
        public AudioClip switched;
        public AudioClip pressed;

        private void Start()
        {
            musicSource.clip = background;
            if (!SceneManager.GetActiveScene().name.StartsWith("PreLevel"))
            {
                musicSource.Play();
            }
        }

        public void ChangeMusic(AudioClip clip)
        {
            musicSource.clip = bossBackground;
            musicSource.Play();
        }
        
        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        private void Update()
        {
            // Add null check for DialogueManager.Instance
            if (Dialogues.DialogueManager.Instance != null)
            {
                if (Dialogues.DialogueManager.Instance.DialogueIsPlaying)
                {
                    musicSource.Pause();
                }
                else
                {
                    musicSource.UnPause();
                }
            }
        }
    }
}
