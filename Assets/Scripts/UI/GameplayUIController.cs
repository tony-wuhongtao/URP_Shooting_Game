using System;
using UnityEditor;
using UnityEngine;
using TonyLearning.ShootingGame.Input;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class GameplayUIController : MonoBehaviour
    {
        [Header("--- Player Input ---")]
        [SerializeField] private PlayerInput _playerInput;
        [Header("--- Canvas ---")]
        [SerializeField] private Canvas hUdCanvas;
        [SerializeField] private Canvas menuCanvas;

        [Header("--- Player Input ---")]
        [SerializeField]private Button _resumeButton;
        [SerializeField]private Button _optionsButton;
        [SerializeField]private Button _mainMenuButton;

        private void OnEnable()
        {
            _playerInput.onPause += Pause;
            _playerInput.onUnPause += Unpause;

            // _resumeButton.onClick.AddListener(OnResumeButtonClick);
            // _optionsButton.onClick.AddListener(OnOptionsButtonClick);
            // _mainMenuButton.onClick.AddListener(onMainMenuButtonClick);
            
            ButtonPressedBehaviour.buttonFuntionTable.Add(_resumeButton.gameObject.name, OnResumeButtonClick);
            ButtonPressedBehaviour.buttonFuntionTable.Add(_optionsButton.gameObject.name, OnOptionsButtonClick);
            ButtonPressedBehaviour.buttonFuntionTable.Add(_mainMenuButton.gameObject.name, onMainMenuButtonClick);

        }

        private void OnDisable()
        {
            _playerInput.onPause -= Pause;
            _playerInput.onUnPause -= Unpause;

            // _resumeButton.onClick.RemoveAllListeners();
            // _optionsButton.onClick.RemoveAllListeners();
            // _mainMenuButton.onClick.RemoveAllListeners();
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            hUdCanvas.enabled = false;
            menuCanvas.enabled = true;
            _playerInput.EnablePauseMenuInput();
            _playerInput.SwitchToDynamicUpdateMode();
            
            UIInput.Instance.SelectUI(_resumeButton);
        }
        public void Unpause()
        {
            _resumeButton.Select();
            _resumeButton.animator.SetTrigger("Pressed");
            // OnResumeButtonClick();
        }

        void OnResumeButtonClick()
        {
            Time.timeScale = 1f;
            hUdCanvas.enabled = true;
            menuCanvas.enabled = false;
            _playerInput.EnableGameplayInput();
            _playerInput.SwitchToFixedUpdateMode();
        }

        void OnOptionsButtonClick()
        {
            //todo
            UIInput.Instance.SelectUI(_optionsButton);
            _playerInput.EnablePauseMenuInput();
        }

        void onMainMenuButtonClick()
        {
            menuCanvas.enabled = false;
            SceneLoader.Instance.LoadMainMainScene();
            
        }
    }
}
