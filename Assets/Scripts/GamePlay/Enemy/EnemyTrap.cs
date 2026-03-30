using UnityEngine;

namespace DungTran31.GamePlay.Enemy
{
    public class EnemyTrap : MonoBehaviour
    {
        [SerializeField] private float damage = 100f;
        [Range(1, 50)]
        [SerializeField] private float speed = 15f;
        [Range(1, 10)]
        [SerializeField] private float lifeTime = 2f;

        private Rigidbody2D rb;

        private void Start()
        {
            // Set the rotation to 180 degrees around the Z axis
            transform.rotation = Quaternion.Euler(0, 0, 180);
            // Ensure the Rigidbody2D component is assigned
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = -transform.up * speed;
            if(lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player.PlayerHealth>().TakeDamage(damage);
                this.gameObject.SetActive(false);
            }
        }
    }
}
