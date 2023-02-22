using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Pool_System;
using TonyLearning.ShootingGame.UI;
using UnityEngine;

namespace TonyLearning.ShootingGame.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] GameObject deathVFX;
        [SerializeField] private AudioData[] deathSFX;
        [Header("--- HEALTH ---")]
        [SerializeField]protected float maxHealth;

        [SerializeField] private bool showOnHeadHealthBar = true;
        [SerializeField] private StatusBar onHeadHealthBar;
        protected float health;

        protected virtual void OnEnable()
        {
            health = maxHealth;
            if (showOnHeadHealthBar)
            {
                ShowOnHeadHealthBar();
            }
            else
            {
                HideOnHeadHealthBar();
            }
        }

        public void ShowOnHeadHealthBar()
        {
            onHeadHealthBar.gameObject.SetActive(true);
            onHeadHealthBar.Initialize(health,maxHealth);
        }

        public void HideOnHeadHealthBar()
        {
            onHeadHealthBar.gameObject.SetActive(false);
        }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;

            if (showOnHeadHealthBar && gameObject.activeSelf)
            {
                onHeadHealthBar.UpdateStatus(health,maxHealth);
            }
            
            if (health <= 0f)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            health = 0f;
            AudioManager.Instance.PlayRandomSFX(deathSFX);
            PoolManager.Release(deathVFX, transform.position);
            gameObject.SetActive(false);
        }

        public virtual void RestoreHealth(float value)
        {
            if (health == maxHealth) return;

            // health += value;
            // health = Mathf.Clamp(health, 0f, maxHealth);
            health = Mathf.Clamp(health + value, 0f, maxHealth);
            if (showOnHeadHealthBar)
            {
                onHeadHealthBar.UpdateStatus(health,maxHealth);
            }
        }

        protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
        {
            while (health < maxHealth)
            {
                yield return waitTime;
                RestoreHealth(maxHealth * percent);
            }
        }
        
        protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
        {
            while (health > 0f)
            {
                yield return waitTime;
                TakeDamage(maxHealth * percent);
            }
        }
    }
}
