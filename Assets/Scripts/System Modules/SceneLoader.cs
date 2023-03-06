using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class SceneLoader : PersistentSingleton<SceneLoader>
    {
        [SerializeField] private Image transitionImage;
        [SerializeField] private float fadeTime = 3.5f;

        private Color _color;
        
        private const string GAMEPLAY = "Gameplay";
        private const string MAINMENU = "MainMenu";
        private const string SCORING = "Scoring";
        void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator LoadingCoroutine(string sceneName)
        {
            var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            loadingOperation.allowSceneActivation = false;
            
            transitionImage.gameObject.SetActive(true);
            while (_color.a < 1f)
            {
                _color.a += Mathf.Clamp01( Time.unscaledDeltaTime / fadeTime );
                transitionImage.color = _color;
                yield return null;
            }

            yield return new WaitUntil((() => loadingOperation.progress >= 0.9f));

            loadingOperation.allowSceneActivation = true;
            
            while (_color.a > 0f)
            {
                _color.a -= Mathf.Clamp01( Time.unscaledDeltaTime / fadeTime );
                transitionImage.color = _color;
                yield return null;
            }
            transitionImage.gameObject.SetActive(false);
        }
        public void LoadGameplayScene()
        {
            StopAllCoroutines();
            StartCoroutine(LoadingCoroutine(GAMEPLAY));
        }

        public void LoadMainMainScene()
        {
            StopAllCoroutines();
            StartCoroutine(LoadingCoroutine(MAINMENU));
        }

        public void LoadScoringScene()
        {
            StopAllCoroutines();
            StartCoroutine(LoadingCoroutine(SCORING));
        }
    }
}
