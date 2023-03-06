using UnityEngine;

namespace TonyLearning.ShootingGame.LootItems
{
    public class MissilePickUp : LootItem
    {
        protected override void PickUp()
        {
            player.PickUpMissile();
            base.PickUp();
        }
    }
}
