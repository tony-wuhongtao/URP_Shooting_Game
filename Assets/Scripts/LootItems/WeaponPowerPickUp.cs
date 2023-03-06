using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.LootItems
{
    public class WeaponPowerPickUp : LootItem
    {
        [SerializeField] AudioData fullPowerPickUpSFX;
        [SerializeField] int fullPowerScoreBonus = 200;

        protected override void PickUp()
        {
            if (player.IsFullPower)
            {
                pickUpSFX = fullPowerPickUpSFX;
                lootMessage.text = $"SCORE + {fullPowerScoreBonus}";
                ScoreManager.Instance.AddScore(fullPowerScoreBonus);
            }
            else
            {
                pickUpSFX = defaultPickUpSFX;
                lootMessage.text = "POWER UP!";
                player.PowerUp();
            }
        
            base.PickUp();
        }
    }
}
