using System;
using TonyLearning.ShootingGame.System_Modules;
using TonyLearning.ShootingGame.UI;
using UnityEngine;

namespace TonyLearning.ShootingGame.Player
{
    public class PlayerEnergy : Singleton<PlayerEnergy>
    {
        [SerializeField] private EnergyBar_HUD energyBar;
        public const int MAX = 100;
        public const int PERCENT = 1;
        private int energy;

        private void Start()
        {
            energyBar.Initialize(energy,MAX);
            ObtainEnergy(MAX);
        }

        public void ObtainEnergy(int value)
        {
            if(energy == MAX) return;
            energy = Mathf.Clamp(energy + value, 0, MAX);
            energyBar.UpdateStatus(energy, MAX);
        }

        public void UseEnergy(int value)
        {
            energy -= value;
            energyBar.UpdateStatus(energy, MAX);
        }

        // public bool IsEnoughEnergy(int value)
        // {
        //    return energy >= value;
        // }
        public bool IsEnoughEnergy(int value) => energy >= value;
    }
}
