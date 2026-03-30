using DungTran31.GamePlay.Enemy;
using System.Collections;
using UnityEngine;

namespace DungTran31.UI
{
    public class ShockwaveManager : MonoBehaviour
    {
        private Coroutine _shockWaveCoroutine;
        private Material _material;
        private Camera _mainCamera;
        [SerializeField] private float _shockWaveTime = 0.75f;
        private static readonly int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
        private static readonly int _ringSpawnPosition = Shader.PropertyToID("_RingSpawnPosition");

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
            _material.SetFloat(_waveDistanceFromCenter, -0.1f);
            _mainCamera = Camera.main; // Cache the main camera reference
        }

        private void OnEnable() => EnemyHealth.OnEnemyDeath += CallShockWave;
        private void OnDisable() => EnemyHealth.OnEnemyDeath -= CallShockWave;

        private void CallShockWave(EnemyHealth.EnemyDeathEventArgs args)
        {
            // Convert world position to screen position using the cached camera reference
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(args.Position);
            // Normalize screen position
            Vector2 normalizedPos = new(screenPos.x / Screen.width, screenPos.y / Screen.height);

            _material.SetVector(_ringSpawnPosition, normalizedPos);

            if (_shockWaveCoroutine != null)
            {
                StopCoroutine(_shockWaveCoroutine);
            }
            _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        }

        private IEnumerator ShockWaveAction(float startPos, float endPos)
        {
            _material.SetFloat(_waveDistanceFromCenter, startPos);

            float elapsedTime = 0;
            while (elapsedTime < _shockWaveTime)
            {
                elapsedTime += Time.deltaTime;
                float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / _shockWaveTime);
                _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
                yield return null;
            }
            _material.SetFloat(_waveDistanceFromCenter, -0.1f);
        }
    }
}
