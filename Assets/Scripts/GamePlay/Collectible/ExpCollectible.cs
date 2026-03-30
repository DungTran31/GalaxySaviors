using DungTran31.GamePlay.Enemy;
using UnityEngine;

namespace DungTran31.GamePlay.Collectible
{
    public class ExpCollectible : MonoBehaviour
    {
        [SerializeField] private int maxExpValue = 5;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player.PlayerExp>().IncreaseExp(Random.Range(1, maxExpValue));
                Destroy(gameObject);
            }
        }
    }
}
