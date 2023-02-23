using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace TonyLearning.ShootingGame.Projectile
{
    public class ProjectileGuidSystem : MonoBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private float minBallisticAngle = -50f;
        [SerializeField] private float maxBallisticAngle = 50f;

        float ballisticAngle;
        private Vector3 targetDirection;
        public IEnumerator HomingCoroutine(GameObject target)
        {
            ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
            while (gameObject.activeSelf)
            {
                if (target.activeSelf)
                {
                    // move to target
                    targetDirection = target.transform.position - transform.position;
                    
                    // rotate to target
                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                    transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                    _projectile.Move();
                }
                else
                {
                    //move direction
                    _projectile.Move();
                }

                yield return null;
            }
        }
    }
}
