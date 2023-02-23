using System.Collections;
using UnityEngine;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class TimeController : Singleton<TimeController>
    {
        [SerializeField,Range(0,1f)] private float bulletTimeScale = 0.1f;

        private float defaultFixedDeltaTime;
        private float t;

        protected override void Awake()
        {
            base.Awake();
            defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        public void BulletTime(float duration)
        {
            Time.timeScale = bulletTimeScale;
            StartCoroutine(SlowOutCoroutine(duration));
        }
        
        public void BulletTime(float inDuration, float outDuration)
        {
            StartCoroutine(SlowInAndOutCoroutine(inDuration,outDuration));
        }
        
        public void BulletTime(float inDuration,float keepDuration, float outDuration)
        {
            StartCoroutine(SlowInKeepAndOutCoroutine(inDuration,keepDuration,outDuration));
        }

        public void SlowIn(float duration)
        {
            StartCoroutine(SlowInCoroutine(duration));
        }

        public void SlowOut(float duration)
        {
            StartCoroutine(SlowOutCoroutine(duration));
        }

        IEnumerator SlowInKeepAndOutCoroutine(float inDuration,float keepDuration, float outDuration)
        {
            yield return StartCoroutine(SlowInCoroutine(inDuration));
            yield return new WaitForSecondsRealtime(keepDuration);
            StartCoroutine(SlowOutCoroutine(outDuration));
        }

        IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
        {
            yield return StartCoroutine(SlowInCoroutine(inDuration));
            StartCoroutine(SlowOutCoroutine(outDuration));
        }
        

        IEnumerator SlowOutCoroutine(float duration)
        {
            t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

                yield return null;
            }
        }
        
        IEnumerator SlowInCoroutine(float duration)
        {
            t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f,bulletTimeScale,  t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

                yield return null;
            }
        }
    }
}
