using System;
using System.Collections;
using TonyLearning.ShootingGame.System_Modules;
using TonyLearning.ShootingGame.UI;
using UnityEngine;

namespace TonyLearning.ShootingGame.Player
{
    public class PlayerEnergy : Singleton<PlayerEnergy>
    {
        [SerializeField] private EnergyBar_HUD energyBar;
        [SerializeField] private float overdriveInterval = 0.1f;

        private bool available = true;
        
        public const int MAX = 100;
        public const int PERCENT = 1;
        private int energy;

        private WaitForSeconds waitForOverdriveInterval;

        private void OnEnable()
        {
            PlayerOverdrive.on += PlayerOverdriveOn;
            PlayerOverdrive.off += PlayerOverdriveOff;
        }


        private void OnDisable()
        {
            PlayerOverdrive.on -= PlayerOverdriveOn;
            PlayerOverdrive.off -= PlayerOverdriveOff;
        }

        protected override void Awake()
        {
            base.Awake();
            waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
        }

        private void Start()
        {
            energyBar.Initialize(energy,MAX);
            ObtainEnergy(MAX);
        }

        public void ObtainEnergy(int value)
        {
            if(energy == MAX || !available || !gameObject.activeSelf) return;
            energy = Mathf.Clamp(energy + value, 0, MAX);
            energyBar.UpdateStatus(energy, MAX);
        }

        public void UseEnergy(int value)
        {
            energy -= value;
            energyBar.UpdateStatus(energy, MAX);
            if (energy == 0 && !available)
            {
                PlayerOverdrive.off.Invoke();
            }
        }

        // public bool IsEnoughEnergy(int value)
        // {
        //    return energy >= value;
        // }
        public bool IsEnoughEnergy(int value) => energy >= value;
        
        private void PlayerOverdriveOn()
        {
            available = false;
            StartCoroutine(nameof(KeepUsingCoroutine));
        }
        private void PlayerOverdriveOff()
        {
            available = true;
            StopCoroutine(nameof(KeepUsingCoroutine));
        }


        IEnumerator KeepUsingCoroutine()
        {
            while (gameObject.activeSelf)
            {
                yield return waitForOverdriveInterval;
                UseEnergy(PERCENT);
            }
        }
    }
}
