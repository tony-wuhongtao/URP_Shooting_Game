using System;
using System.Collections;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    public class EnemyProjectile_Aming : Projectile
    {
        private void Awake()
        {
            SetTarget(GameObject.FindGameObjectWithTag("Player"));
        }

        protected override void OnEnable()
        {
            StartCoroutine(nameof(MoveDirectionCoroutine)); // wait for one frame to get target position
            base.OnEnable();
        }

        IEnumerator MoveDirectionCoroutine()
        {
            yield return null;

            if (target.activeSelf)
            {
                moveDirection = (target.transform.position - transform.position).normalized;
            }
        }
    }
}
