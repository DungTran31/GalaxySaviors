using DungTran31.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DungTran31
{
    public class LevelBox : MonoBehaviour
    {
        [SerializeField] private int levelId;
        [SerializeField] private GameObject visualCue;
        [SerializeField] private GameObject lockObject;
        [SerializeField] private GameObject numberObject;
        [SerializeField] private Sprite[] digitSprites;

        private Button button;
        private SpriteRenderer numberSpriteRenderer;
        private bool isPlayerNearby;

        private void Awake()
        {
            visualCue.SetActive(false);
            button = GetComponent<Button>();
            numberSpriteRenderer = numberObject.GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            bool isUnlocked = LevelManager.IsBossUnlocked(levelId);

            button.interactable = isUnlocked;
            lockObject.SetActive(!isUnlocked);
            numberObject.SetActive(isUnlocked);
            if (isUnlocked)
            {
                SetNumberSprite(levelId);
            }
        }

        private void SetNumberSprite(int level)
        {
            if (numberSpriteRenderer == null) return;

            numberSpriteRenderer.sprite = digitSprites[level - 1];

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                visualCue.SetActive(true);
                isPlayerNearby = true;
            }
        }

        private void Update()
        {
            // Read input in Update so key presses are not missed.
            if (!isPlayerNearby) return;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!LevelManager.IsBossUnlocked(levelId)) return;

                SceneManager.LoadScene("PreLevel" + levelId);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                visualCue.SetActive(false);
                isPlayerNearby = false;
            }
        }
    }
}