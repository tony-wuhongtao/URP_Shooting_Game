using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Characters.Enemies;
using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    public class PlayerMissile : PlayerProjectileOverdrive
    {
        [SerializeField] private AudioData targetAcquiredVoice = null;
        [Header("--- Speed ---")]
        [SerializeField] float lowSpeed = 8f;
        [SerializeField] float highSpeed = 25f;
        [SerializeField] float variableSpeedDelay = 0.5f;

        [Header("--- Explosion ---")]
        [SerializeField] private GameObject explosionVFX = null;
        [SerializeField] private AudioData explosionSFX = null;
        [SerializeField] private LayerMask enemyLayerMask = default;
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private float explosionDamage = 100f;

        private WaitForSeconds waitVariableSpeedDelay;

        protected override void Awake()
        {
            base.Awake();
            waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(nameof(VariableSpeedCoroutine));
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            //spawn a explosion VFX and SFX
            PoolManager.Release(explosionVFX, transform.position);
            AudioManager.Instance.PlayRandomSFX(explosionSFX);
            //Enemies in explosion take AOE damage
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(explosionDamage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        IEnumerator VariableSpeedCoroutine()
        {
            moveSpeed = lowSpeed;
            yield return waitVariableSpeedDelay;

            moveSpeed = highSpeed;

            if (target != null)
            {
                AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);
            }
        }
    }
}
