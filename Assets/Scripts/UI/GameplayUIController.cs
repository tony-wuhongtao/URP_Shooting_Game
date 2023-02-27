using System;
using TonyLearning.ShootingGame.Audio;
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

        [Header("--- Audio DATA ---")] 
        [SerializeField]private AudioData pauseSFX;
        [SerializeField]private AudioData unpauseSFX;
        [Header("--- Canvas ---")]
        [SerializeField] private Canvas hUdCanvas;
        [SerializeField] private Canvas menuCanvas;

        [Header("--- Player Input ---")]
        [SerializeField]private Button _resumeButton;
        [SerializeField]private Button _optionsButton;
        [SerializeField]private Button _mainMenuButton;

        private int buttonPressedParameterID = Animator.StringToHash("Pressed");

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
            ButtonPressedBehaviour.buttonFuntionTable.Clear();
        }

        private void Pause()
        {
            TimeController.Instance.Pause();
            hUdCanvas.enabled = false;
            menuCanvas.enabled = true;
            GameManager.GameState = GameState.Paused;
            _playerInput.EnablePauseMenuInput();
            _playerInput.SwitchToDynamicUpdateMode();
            
            UIInput.Instance.SelectUI(_resumeButton);
            AudioManager.Instance.PlaySFX(pauseSFX);
        }
        public void Unpause()
        {
            _resumeButton.Select();
            _resumeButton.animator.SetTrigger(buttonPressedParameterID);
            AudioManager.Instance.PlaySFX(unpauseSFX);
            // OnResumeButtonClick();
        }

        void OnResumeButtonClick()
        {
            TimeController.Instance.Unpause();
            hUdCanvas.enabled = true;
            menuCanvas.enabled = false;
            GameManager.GameState = GameState.Playing;
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
