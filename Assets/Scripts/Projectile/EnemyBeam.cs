using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    public class EnemyBeam : MonoBehaviour
    {
        [SerializeField] float damage = 50f;
        [SerializeField] GameObject hitVFX;
        
        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player.Player>(out Player.Player player))
            {
                player.TakeDamage(damage);
                PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            }
        }
    }
}
