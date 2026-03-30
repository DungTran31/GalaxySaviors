using DungTran31.GamePlay.Enemy.SO;
using System;
using UnityEngine;

namespace DungTran31.GamePlay.Collectible
{
    public class HealthCollectible : MonoBehaviour
    {
        [SerializeField] private GameObject floatingHealthTextPrefab;
        [SerializeField] private float healthValue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                collision.GetComponent<Player.PlayerHealth>().Heal(healthValue);
                ShowHealthIncrease(healthValue.ToString());
                Destroy(gameObject);
            }
        }

        private void ShowHealthIncrease(string text)
        {
            GameObject floatingText = Instantiate(floatingHealthTextPrefab, transform.position, Quaternion.identity);
            floatingText.GetComponentInChildren<TextMesh>().text = text;
        }
    }
}
