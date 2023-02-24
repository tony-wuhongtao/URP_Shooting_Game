using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class StatusBar : MonoBehaviour
    {
        [SerializeField] private Image fillImageBack;
        [SerializeField] private Image fillImageFront;

        [SerializeField] private bool delayFill = true;
        [SerializeField] private float fillDely = 0.5f;
        
        [SerializeField] private float fillSpeed = 0.1f;
        private float currentFillAmount;
        protected float targetFillAmount;
        private float previousFillAmount;
        private float t;

        private WaitForSeconds waitForDelayFill;
        private Coroutine bufferedFillingCoroutine;

        private void Awake()
        {
            if (TryGetComponent<Canvas>(out Canvas canvas))
            {
                canvas.worldCamera = Camera.main;
            }
     
            waitForDelayFill = new WaitForSeconds(fillDely);

        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public virtual void Initialize(float currentValue, float maxValue)
        {
            currentFillAmount = currentValue / maxValue;
            targetFillAmount = currentFillAmount;
            fillImageBack.fillAmount = currentFillAmount;
            fillImageFront.fillAmount = currentFillAmount;
        }

        public void UpdateStatus(float currentValue, float maxValue)
        {
            targetFillAmount = currentValue / maxValue;
            if (bufferedFillingCoroutine != null)
            {
                StopCoroutine(bufferedFillingCoroutine);
            }
            // if status reduce
            if (currentFillAmount > targetFillAmount)
            {
                // fill image front = targit fill amount
                fillImageFront.fillAmount = targetFillAmount;
                //slowly reduce fill image back fill amount
                bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
                return;
            }

            if (currentFillAmount < targetFillAmount)
            {
                fillImageBack.fillAmount = targetFillAmount;
                bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
            }
            
            
        }

        protected virtual IEnumerator BufferedFillingCoroutine(Image image)
        {
            if (delayFill)
            {
                yield return waitForDelayFill;
            }

            previousFillAmount = currentFillAmount;
            t = 0f;
            while (t<1f)
            {
                t += Time.deltaTime * fillSpeed;
                currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
                image.fillAmount = currentFillAmount;
                yield return null;
            }
        }
    }
}
