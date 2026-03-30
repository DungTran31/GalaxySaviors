using UnityEngine;
using Cinemachine;
using DungTran31.GamePlay.Enemy;

namespace DungTran31.UI
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float ShakeIntensity = 20f;
        [SerializeField] private float ShakeTime = 0.2f;

        private float timer;
        private CinemachineBasicMultiChannelPerlin perlin;

        private void Awake()
        {
            if (virtualCamera == null)
            {
                virtualCamera = GetComponent<CinemachineVirtualCamera>();
            }
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            timer = 0;
            perlin.m_AmplitudeGain = 0f;
        }

        private void OnEnable()
        {
            EnemyHealth.OnEnemyDeath += ShakeCamera;
        }

        private void OnDisable()
        {
            EnemyHealth.OnEnemyDeath -= ShakeCamera;
        }

        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.O))
            //{
            //    ShakeCamera();
            //}


            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    perlin.m_AmplitudeGain = 0f;
                }
            }
        }

        // Overloaded method for manual triggering without parameters
        //private void ShakeCamera()
        //{
        //    perlin.m_AmplitudeGain = ShakeIntensity;
        //    timer = ShakeTime;
        //}

        // Original method, now explicitly handling the event with parameters
        private void ShakeCamera(EnemyHealth.EnemyDeathEventArgs args)
        {
            perlin.m_AmplitudeGain = ShakeIntensity;
            timer = ShakeTime;
        }
    }
}
