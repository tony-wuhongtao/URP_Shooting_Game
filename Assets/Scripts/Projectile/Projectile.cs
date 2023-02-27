using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Characters;
using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Projectile
{
    
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitVFX;
        [SerializeField] private AudioData[] hitSFX;
        [SerializeField] float damage;
        [SerializeField] protected float moveSpeed = 10f;
        [SerializeField] protected Vector2 moveDirection;

        protected GameObject target;

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<Character>(out Character character))
            {
                character.TakeDamage(damage);
                var contactPoint = col.GetContact(0);
                PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
                AudioManager.Instance.PlayRandomSFX(hitSFX);
                gameObject.SetActive(false);
            }
            
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(MoveDirectly());
        }
        
        IEnumerator MoveDirectly()
        {
            while (gameObject.activeSelf)
            {
                Move();
                yield return null;
            }
        }

        protected void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public void Move()
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        
        
    }
}
