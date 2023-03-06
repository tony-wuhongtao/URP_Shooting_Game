using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.LootItems
{
    public class ScoreBonusPickUp : LootItem
    {
        [SerializeField] private int scoreBonus;
        protected override void PickUp()
        {
            ScoreManager.Instance.AddScore(scoreBonus);
            base.PickUp();
            
        }
    }
}
