using System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    public class PlayerProjectile : Projectile
    {
        private TrailRenderer trail;

        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();

            if (moveDirection != Vector2.right)
            {
                transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, moveDirection);
            }
        }

        private void OnDestroy()
        {
            trail.Clear();
        }
    }
}
