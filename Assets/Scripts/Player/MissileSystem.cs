using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Pool_System;
using TonyLearning.ShootingGame.UI;
using UnityEngine;


namespace TonyLearning.ShootingGame.Player
{
    public class MissileSystem : MonoBehaviour
    {
        [SerializeField] private int defaultAmount = 5;
        [SerializeField] private float cooldownTime = 1f; 
        [SerializeField] private GameObject missilePrefab = null;
        [SerializeField] private AudioData launchSFX = null;

        private bool isReady = true;
        private int amount;
        

        private void Awake()
        {
            amount = defaultAmount;
        }

        private void Start()
        {
            MissileDisplay.UpdateAmountText(amount);
        }

        public void Launch(Transform muzzleTransform)
        {
            if(amount == 0 || !isReady) return;

            isReady = false;
            //Release a missile clone from object pool
            PoolManager.Release(missilePrefab, muzzleTransform.position);
            //play missile launch SFX
            AudioManager.Instance.PlayRandomSFX(launchSFX);
            amount--;
            MissileDisplay.UpdateAmountText(amount);

            if (amount == 0)
            {
                MissileDisplay.UpdateCooldownImage(1f);
            }
            else
            {
                //Cooldown missile launching
                StartCoroutine(nameof(CooldownCoroutine));
            }
        }

        IEnumerator CooldownCoroutine()
        {
            var cooldownValue = cooldownTime;
            while (cooldownValue > 0)
            {
                MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
                cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);
                yield return null;
            }
            yield return new WaitForSeconds(cooldownTime);
            isReady = true;
        }
    }
}
