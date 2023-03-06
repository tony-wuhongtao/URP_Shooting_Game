using System;
using TonyLearning.ShootingGame.LootItems;
using TonyLearning.ShootingGame.Player;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.Characters.Enemies
{
    public class Enemy : Character
    {
        [SerializeField]private int scorePoint = 100;
        [SerializeField]private int deathEnergyBonus = 3;
        [SerializeField] protected int healthFactor;
        
        LootSpawner lootSpawner;
        
        protected virtual void Awake()
        {
            lootSpawner = GetComponent<LootSpawner>();
        }
        protected override void OnEnable()
        {
            SetHealth();
            base.OnEnable();
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<Player.Player>(out Player.Player player))
            {
                player.Die();
                Die();
            }
        }

        public override void Die()
        {
            ScoreManager.Instance.AddScore(scorePoint);
            PlayerEnergy.Instance.ObtainEnergy(deathEnergyBonus);
            EnermyManager.Instance.RemoveFromList(gameObject);
            lootSpawner.Spawn(transform.position);
            base.Die();
            
        }
        
        protected virtual void SetHealth()
        {
            maxHealth += (int)(EnermyManager.Instance.WaveNumber / healthFactor);
        }
    }
}
