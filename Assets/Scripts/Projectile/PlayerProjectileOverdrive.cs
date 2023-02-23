using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    public class PlayerProjectileOverdrive : PlayerProjectile
    {
        [SerializeField] private ProjectileGuidSystem _guidSystem;
        protected override void OnEnable()
        {
            SetTarget(EnermyManager.Instance.RandomEnemy);
            transform.rotation = Quaternion.identity;
            if (target == null)
            {
                base.OnEnable();
            }
            else
            {
                //Tracke target
                StartCoroutine(_guidSystem.HomingCoroutine(target));
            }
        }
    }
}
