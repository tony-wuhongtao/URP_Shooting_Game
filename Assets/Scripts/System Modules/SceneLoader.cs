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
        void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator LoadCoroutine(string sceneName)
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
            StartCoroutine(LoadCoroutine(GAMEPLAY));
        }
    }
}
