using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class StatusBar_HUD : StatusBar
    {
        [SerializeField] private Text percentText;

        void SetPercentText()
        {
            percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
        }

        public override void Initialize(float currentValue, float maxValue)
        {
            base.Initialize(currentValue, maxValue);
            SetPercentText();
        }

        protected override IEnumerator BufferedFillingCoroutine(Image image)
        {
            SetPercentText();
            return base.BufferedFillingCoroutine(image);
        }
    }
}
