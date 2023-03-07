using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Header("--- Canvas ---")]
        [SerializeField] private Canvas mainMenuCanvas;
        [Header("--- Buttons ---")]
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonOptions;
        [SerializeField] private Button buttonQuit;


        private void OnEnable()
        {
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonStart.gameObject.name, OnStartGameButtonClick);
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonOptions.gameObject.name, OnOptionsClicked);
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonQuit.gameObject.name, OnQuitClicked);
        }

        private void OnDisable()
        {
            ButtonPressedBehaviour.buttonFuntionTable.Clear();
        }

        private void Start()
        {
            Time.timeScale = 1f;
            GameManager.GameState = GameState.Playing;
            UIInput.Instance.SelectUI(buttonStart);
        }

        void OnStartGameButtonClick()
        {
            mainMenuCanvas.enabled = false;
            SceneLoader.Instance.LoadGameplayScene();
        }

        void OnOptionsClicked()
        {
            UIInput.Instance.SelectUI(buttonOptions);
            SceneLoader.Instance.LoadOptionsScene();
        }

        void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
