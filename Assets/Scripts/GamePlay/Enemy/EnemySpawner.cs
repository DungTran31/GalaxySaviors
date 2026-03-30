using System.Collections;
using UnityEngine;
using DungTran31.Utilities;


namespace DungTran31.GamePlay.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject player; // Reference to the player GameObject
        [SerializeField] private float avoidanceRadius = 5f; // Radius to avoid spawning near the player
        [SerializeField] private Vector2 spawnAreaMin;
        [SerializeField] private Vector2 spawnAreaMax;
        [SerializeField] private float initialSpawnRate = 10f;
        // Minimum spawn rate to ensure it doesn't get too fast
        [SerializeField] private float minSpawnRate = 1f;
        // Add a factor to control how much the spawn rate decreases
        [SerializeField] private float spawnRateDecreaseFactor = 0.9f;
        [SerializeField] private int maxEnemies = 15;
        private bool canSpawn = true;
        private int currentEnemyCount = 0;

        private void Start()
        {
            StartCoroutine(Spawner());
        }

        private void OnEnable() => BossHealth.OnBossDeath += StopSpawning;

        private void StopSpawning(BossHealth.BossDeathEventArgs args)
        {
            canSpawn = false; // Stop spawning new enemies

            // Find and deactivate all enemies
            DeactivateAllEnemiesWithTag("Enemy");
            DeactivateAllEnemiesWithTag("EnemyBullet");
            DeactivateAllEnemiesWithTag("Collectible");
            DeactivateAllEnemiesWithTag("Bullet");
        }

        private void DeactivateAllEnemiesWithTag(string tag)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    // Check if the EnemyHealth component exists before calling Die()
                    if (gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth)) // because boss have Enemy tag but don't have EnemyHealth script
                    {
                        enemyHealth.Die();
                    }
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator Spawner()
        {
            float currentSpawnRate = initialSpawnRate;

            while (canSpawn && currentEnemyCount <= maxEnemies)
            {
                WaitForSeconds wait = new(currentSpawnRate);
                yield return wait;

                if (player == null)
                {
                    Debug.Log("Player GameObject is null. Stopping the spawner.");
                    yield break; // Exit the coroutine early
                }

                Vector3 spawnPosition = Vector3.zero;
                bool positionValid = false;
                while (!positionValid)
                {
                    float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                    float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                    spawnPosition = new Vector3(randomX, randomY, 0);
                    
                    if (Vector3.Distance(spawnPosition, player.transform.position) > avoidanceRadius)
                    {
                        positionValid = true;
                    }
                }

                int index = Random.Range(0, _enemyPrefabs.Length);
                string enemyTag = "";
                switch (index)
                {
                    case 0: enemyTag = "enemy"; break;
                    case 1: enemyTag = "enemy2"; break;
                    case 2: enemyTag = "rangedEnemy"; break;
                }

                ObjectPooler.Instance.SpawnFromPool(enemyTag, spawnPosition, Quaternion.identity);
                currentEnemyCount++;

                // Decrease the spawn rate for the next spawn, but ensure it doesn't go below minSpawnRate
                currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate * spawnRateDecreaseFactor);
            }
        }
    }
}
