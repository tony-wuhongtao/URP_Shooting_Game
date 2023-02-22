using System;
using System.Collections;
using TonyLearning.ShootingGame.Characters;
using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitVFX;
        [SerializeField] float damage;
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] protected Vector2 moveDirection;

        protected GameObject target;
        IEnumerator MoveDirectly()
        {
            while (gameObject.activeSelf)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<Character>(out Character character))
            {
                character.TakeDamage(damage);
                var contactPoint = col.GetContact(0);
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
                gameObject.SetActive(false);
            }
            
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(MoveDirectly());
        }
    }
}
