using System;
using System.Net.Mime;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class WaveUI : MonoBehaviour
    {
        private Text waveText;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            waveText = GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            waveText.text = "- WAVE " + EnermyManager.Instance.WaveNumber + " -";
        }
    }
}
