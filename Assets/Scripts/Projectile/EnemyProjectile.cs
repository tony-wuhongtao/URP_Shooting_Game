using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TonyLearning.ShootingGame.Projectile
{
    public class EnemyProjectile : Projectile
    {
        private void Awake()
        {
            if (moveDirection != Vector2.left)
            {
                transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
            }
        }
    }
}
