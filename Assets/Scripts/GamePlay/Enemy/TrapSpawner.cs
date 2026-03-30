using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class TrapSpawner : MonoBehaviour
    {
        [Header("Trap Settings")]
        [SerializeField] private GameObject fireTrapPrefab;
        [SerializeField] private float spawnInterval = 5f; // Interval between spawns

        private Coroutine spawnCoroutine;

        private void OnEnable()
        {
            BossHealth.OnBossDeath += HandleBossDeath;
        }

        private void OnDisable()
        {
            BossHealth.OnBossDeath -= HandleBossDeath;
        }

        private void Start()
        {
            // Start the coroutine to spawn traps every 5 seconds
            spawnCoroutine = StartCoroutine(SpawnTrap());
        }

        private IEnumerator SpawnTrap()
        {
            while (true)
            {
                // Spawn the fire trap at the specified spawn point
                Instantiate(fireTrapPrefab, this.transform.position, this.transform.rotation);

                // Wait for the specified interval before spawning the next trap
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void HandleBossDeath(BossHealth.BossDeathEventArgs args)
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
        }
    }
}
