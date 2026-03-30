using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyWind : MonoBehaviour
    {
        // How often (seconds) the player is pushed
        [SerializeField] private float pushInterval = 5f;
        // Force magnitude applied to the player (impulse)
        [SerializeField] private float pushForce = 10f;

        // Optional particle system (e.g. leaves) that should be nudged left when the wind pushes
        [SerializeField] private ParticleSystem leafParticles;
        // How much velocity to add to each particle when nudging leaves
        [SerializeField] private float leafPushForce = 1f;

        private float timer;

        // Cache for player rigidbody
        private Rigidbody2D playerRb;

        // Reusable buffer for particles
        private ParticleSystem.Particle[] particleBuffer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            TryFindPlayerRigidbody();
            timer = 0f;

            if (leafParticles != null)
            {
                var main = leafParticles.main;
                particleBuffer = new ParticleSystem.Particle[main.maxParticles];
            }
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= pushInterval)
            {
                ApplyLeftPushToPlayer();
                ApplyLeftPushToParticles();
                timer = 0f;
            }
        }

        private void TryFindPlayerRigidbody()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerRb = player.GetComponent<Rigidbody2D>();
            }
            else
            {
                playerRb = null;
            }
        }

        private void ApplyLeftPushToPlayer()
        {
            if (playerRb == null)
            {
                TryFindPlayerRigidbody();
                if (playerRb == null) return;
            }

            // Apply a leftward impulse to the player
            playerRb.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
        }

        private void ApplyLeftPushToParticles()
        {
            if (leafParticles == null) return;

            // Ensure buffer exists and is large enough
            var main = leafParticles.main;
            if (particleBuffer == null || particleBuffer.Length < main.maxParticles)
            {
                particleBuffer = new ParticleSystem.Particle[main.maxParticles];
            }

            int count = leafParticles.GetParticles(particleBuffer);
            if (count == 0) return;

            // Compute push vector in the correct simulation space
            Vector3 worldPush = Vector3.left * leafPushForce;
            Vector3 push;
            if (main.simulationSpace == ParticleSystemSimulationSpace.Local)
            {
                // Convert world push into particle system local space
                push = leafParticles.transform.InverseTransformDirection(worldPush);
            }
            else
            {
                // World space: use world push directly (particle velocity is in world coordinates)
                push = worldPush;
            }

            // Apply a one-time velocity nudge to existing particles
            for (int i = 0; i < count; i++)
            {
                // ParticleSystem.Particle.velocity is a Vector3
                particleBuffer[i].velocity += push;
            }

            leafParticles.SetParticles(particleBuffer, count);
        }
    }
}