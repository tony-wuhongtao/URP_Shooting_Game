using System;
using TonyLearning.ShootingGame.System_Modules;
using TonyLearning.ShootingGame.UI;
using UnityEngine;

namespace TonyLearning.ShootingGame.Characters.Enemies
{
    public class Boss : Enemy
    {
        private BossHealthBar _healthBar;
        private Canvas _heathBarCanvas;

        protected override void Awake()
        {
            base.Awake();
            _healthBar = FindObjectOfType<BossHealthBar>();
            _heathBarCanvas = _healthBar.GetComponentInChildren<Canvas>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _healthBar.Initialize(health,maxHealth);
            _heathBarCanvas.enabled = true;
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<Player.Player>(out Player.Player player))
            {
                player.Die();
            }
        }
        

        public override void Die()
        {
            _heathBarCanvas.enabled = false;
            base.Die();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            _healthBar.UpdateStatus(health,maxHealth);
        }
        
        protected override void SetHealth()
        {
            maxHealth += EnermyManager.Instance.WaveNumber * healthFactor;
        }
    }
}
