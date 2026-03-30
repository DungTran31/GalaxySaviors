using System.Collections;
using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyBlackHoleSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        public GameObject blackHolePrefab;

        [Header("Spawn Area (min/max)")]
        public Vector3 spawnMin = new Vector3(-5f, 0f, -5f);
        public Vector3 spawnMax = new Vector3(5f, 0f, 5f);

        [Header("Spawn Settings")]
        [Tooltip("How many black holes to spawn each wave")]
        public int spawnCount = 5;                   // 5 black holes per wave
        public bool spawnOnStart = true;
        public float spawnInterval = 5f;             // 5 seconds between waves

        [Header("Camera Spawning")]
        public bool spawnInsideCamera = true;        // when true spawn inside camera viewport
        public Camera spawnCamera;                   // optional camera reference; falls back to Camera.main
        public float spawnDistanceFromCamera = 10f;  // distance in front of camera (used as Z for ViewportToWorldPoint)

        void Start()
        {
            if (spawnOnStart && blackHolePrefab != null)
            {
                StartCoroutine(SpawnRoutine());
            }
        }

        public void SpawnOnce()
        {
            if (blackHolePrefab == null)
            {
                Debug.LogWarning("BlackHole prefab is not assigned to the spawner.", this);
                return;
            }

            Vector3 randomPos;

            if (spawnInsideCamera)
            {
                Camera cam = spawnCamera != null ? spawnCamera : Camera.main;
                if (cam != null)
                {
                    // Random point inside camera viewport [0,1] x [0,1]
                    float vx = Random.Range(0f, 1f);
                    float vy = Random.Range(0f, 1f);

                    // Use spawnDistanceFromCamera as the z (distance from camera) for ViewportToWorldPoint.
                    // This works for both perspective and orthographic cameras.
                    Vector3 viewportPoint = new Vector3(vx, vy, spawnDistanceFromCamera);
                    randomPos = cam.ViewportToWorldPoint(viewportPoint);
                }
                else
                {
                    // Fallback to spawn area if no camera is available
                    Debug.LogWarning("No camera available for spawnInsideCamera. Falling back to spawnMin/spawnMax.", this);
                    randomPos = new Vector3(
                        Random.Range(spawnMin.x, spawnMax.x),
                        Random.Range(spawnMin.y, spawnMax.y),
                        Random.Range(spawnMin.z, spawnMax.z)
                    );
                }
            }
            else
            {
                // Original behavior: spawn inside defined spawn box
                randomPos = new Vector3(
                    Random.Range(spawnMin.x, spawnMax.x),
                    Random.Range(spawnMin.y, spawnMax.y),
                    Random.Range(spawnMin.z, spawnMax.z)
                );
            }

            Instantiate(blackHolePrefab, randomPos, Quaternion.identity);
        }

        IEnumerator SpawnRoutine()
        {
            // Every spawnInterval seconds spawn "spawnCount" black holes
            while (true)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnOnce();
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}   