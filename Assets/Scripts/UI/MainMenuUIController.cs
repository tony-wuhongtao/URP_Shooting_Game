using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button buttonStartGame;


        private void OnEnable()
        {
            buttonStartGame.onClick.AddListener(OnStartGameButtonClick);
        }

        private void OnDisable()
        {
            buttonStartGame.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            Time.timeScale = 1f;
            GameManager.GameState = GameState.Playing;
        }

        void OnStartGameButtonClick()
        {
            SceneLoader.Instance.LoadGameplayScene();
        }
    }
}
