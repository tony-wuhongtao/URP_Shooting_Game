using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class StatusBar_HUD : StatusBar
    {
        [SerializeField] protected Text percentText;

        protected virtual void SetPercentText()
        {
            // percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
            percentText.text = targetFillAmount.ToString("P0");
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
