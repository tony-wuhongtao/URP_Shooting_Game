using TonyLearning.ShootingGame.Player;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.Characters.Enemies
{
    public class Enemy : Character
    {
        [SerializeField]private int scorePoint = 100;
        [SerializeField]private int deathEnergyBonus = 3;

        public override void Die()
        {
            ScoreManager.Instance.AddScore(scorePoint);
            PlayerEnergy.Instance.ObtainEnergy(deathEnergyBonus);
            EnermyManager.Instance.RemoveFromList(gameObject);
            base.Die();
            
        }
    }
}
