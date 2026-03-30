using DungTran31.GamePlay.Enemy;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace DungTran31.UI
{
    public class CinematicBars : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float zoomSize = 5f; // Target size for zoom effect, adjust as needed
        [SerializeField] private float zoomDuration = 1f; // Duration of the zoom effect
        private RectTransform topBar, bottomBar;
        private float changeSizeAmount;
        private float targetSize;
        private bool isActive;

        public static event Action OnCinematicBarsFinished;

        private void Awake()
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            GameObject gameObject = new("topBar", typeof(Image));
            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            topBar = gameObject.GetComponent<RectTransform>();
            topBar.anchorMin = new Vector2(0, 1);
            topBar.anchorMax = new Vector2(1, 1);
            topBar.sizeDelta = new Vector2(0, 0);

            gameObject = new GameObject("bottomBar", typeof(Image));
            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            bottomBar = gameObject.GetComponent<RectTransform>();
            bottomBar.anchorMin = new Vector2(0, 0);
            bottomBar.anchorMax = new Vector2(1, 0);
            bottomBar.sizeDelta = new Vector2(0, 0);
        }


        private void OnEnable()
        {
            topBar.gameObject.SetActive(true);
            bottomBar.gameObject.SetActive(true);
            BossHealth.OnBossDeath += ControlCinematicBars;
        }

        private void OnDisable()
        {
            topBar.gameObject.SetActive(false);
            bottomBar.gameObject.SetActive(false);
            BossHealth.OnBossDeath -= ControlCinematicBars;
        }

        private IEnumerator ControlCinematicBarsCoroutine(BossHealth.BossDeathEventArgs _)
        {
            yield return new WaitForSeconds(3f); // Wait for 1 second before showing the bars

            Show(500, 1f);
            // Correctly using OrthographicSize for an orthographic camera
            float originalSize = virtualCamera.m_Lens.OrthographicSize; // Original size of the camera

            // Zoom in
            float elapsedTime = 0;
            while (elapsedTime < zoomDuration)
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(originalSize, zoomSize, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before hiding the bars

            // Zoom out back to original size
            elapsedTime = 0;
            while (elapsedTime < zoomDuration)
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(zoomSize, originalSize, elapsedTime / zoomDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Hide(0.5f);
            OnCinematicBarsFinished?.Invoke();
        }


        private void ControlCinematicBars(BossHealth.BossDeathEventArgs args)
        {
            StartCoroutine(ControlCinematicBarsCoroutine(args));
        }

        private void Update()
        {
            if (isActive)
            {
                Vector2 sizeDelta = topBar.sizeDelta;
                sizeDelta.y += changeSizeAmount * Time.deltaTime;
                if (changeSizeAmount > 0)
                {
                    if (sizeDelta.y >= targetSize)
                    {
                        sizeDelta.y = targetSize;
                        isActive = false;
                    }
                }
                else
                {
                    if (sizeDelta.y <= targetSize)
                    {
                        sizeDelta.y = targetSize;
                        isActive = false;
                    }
                }
                topBar.sizeDelta = sizeDelta;
                bottomBar.sizeDelta = sizeDelta;
            }
        }

        private void Show(float targetSize, float time)
        {
            this.targetSize = targetSize;
            changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
            isActive = true;
        }

        private void Hide(float time)
        {
            targetSize = 0;
            changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
            isActive = true;
        }
    }
}
