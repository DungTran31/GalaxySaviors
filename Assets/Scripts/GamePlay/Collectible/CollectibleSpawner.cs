using DungTran31.GamePlay.Enemy;
using UnityEngine;

namespace DungTran31.GamePlay.Collectible
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] collectiblePrefabs; // Now an array of GameObjects for random spawning
        [SerializeField] private GameObject expCollectiblePrefab;
        [SerializeField] private Vector2 spawnAreaMin; // Minimum spawn coordinates
        [SerializeField] private Vector2 spawnAreaMax; // Maximum spawn coordinates
        [SerializeField] private float spawnRate = 5f; // Time in seconds between each spawn
        private float nextSpawnTime;

        private void Start()
        {
            nextSpawnTime = Time.time + spawnRate;
        }

        private void Update()
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnCollectible();
                nextSpawnTime = Time.time + spawnRate;
            }
        }

        private void OnEnable() => EnemyHealth.OnEnemyDeath += SpawnExp;

        private void OnDisable() => EnemyHealth.OnEnemyDeath -= SpawnExp;

        private void SpawnExp(EnemyHealth.EnemyDeathEventArgs args)
        {
            Instantiate(expCollectiblePrefab, args.Position, Quaternion.identity);
        }

        private void SpawnCollectible()
        {
            // Generate a random position within the defined boundaries
            float spawnX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float spawnY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new(spawnX, spawnY);

            // Choose a random collectible prefab from the array
            if (collectiblePrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, collectiblePrefabs.Length);
                GameObject collectiblePrefab = collectiblePrefabs[randomIndex];

                // Instantiate the chosen collectible at the random position
                Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
