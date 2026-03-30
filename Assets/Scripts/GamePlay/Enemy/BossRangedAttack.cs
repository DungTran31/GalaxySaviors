using UnityEngine;
using DungTran31.Core;
using DungTran31.Utilities;
using System.Collections.Generic;

namespace DungTran31.GamePlay.Enemy
{
    public class BossRangedAttack : MonoBehaviour
    {
        [SerializeField] private Transform[] firingPoints;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float distanceToShoot = 10f;
        [SerializeField] private string projectilePoolTag = "enemyBullet";

        private Transform target;
        private float timeToFire;

        private void Start()
        {
            GetTarget();
        }

        private void Update()
        {
            if (UIManager.IsGamePaused) return;
            if (Dialogues.DialogueManager.Instance.DialogueIsPlaying) return;

            if (!target)
            {
                GetTarget();
                return;
            }

            HandleRangedAttack();
        }

        private void GetTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void HandleRangedAttack()
        {
            if (firingPoints == null || firingPoints.Length == 0 || target == null) return;

            float distance = Vector2.Distance(transform.position, target.position);
            if (distance > distanceToShoot) return;


            if (timeToFire <= 0f)
            {
                FireFromAllPoints();
                timeToFire = fireRate;
            }
            else
            {   
                timeToFire -= Time.deltaTime;
            }
        }


        private void FireFromAllPoints()
        {
            foreach (var fp in firingPoints)
            {
                if (fp == null) continue;

                // Bullet will move along its own forward/right direction from this initial rotation.
                ObjectPooler.Instance.SpawnFromPool(projectilePoolTag, fp.position, fp.rotation);
            }
        }
    }
}