using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Prefabs.Loot_Items
{
    [System.Serializable] public class LootSetting
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float dropPercentage;

        public void Spawn(Vector3 position)
        {
            if (Random.Range(0f, 100f) <= dropPercentage)
            {
                PoolManager.Release(prefab, position);
            }
        }
    }
}
