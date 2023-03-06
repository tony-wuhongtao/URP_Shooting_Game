using System;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using PlayerInput = TonyLearning.ShootingGame.Input.PlayerInput;

namespace TonyLearning.ShootingGame.UI
{
    public class GameOverScreenUI : MonoBehaviour
    {
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Canvas HUDCanvas;
        [SerializeField] private AudioData confirmGameOverSound;

        private int exitStateID = Animator.StringToHash("GameOverScreenExit");

        private Canvas _canvas;
        private Animator _animator;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _animator = GetComponent<Animator>();

            _canvas.enabled = false;
            _animator.enabled = false;
        }

        private void OnEnable()
        {
            GameManager.onGameOver += onGameOver;
            _input.onConfirmGameOver += onConfirmGameOver;
        }


        private void OnDisable()
        {
            GameManager.onGameOver -= onGameOver;
            _input.onConfirmGameOver -= onConfirmGameOver;
        }

        private void onGameOver()
        {
            HUDCanvas.enabled = false;
            _canvas.enabled = true;
            _animator.enabled = true;
            _input.DisableAllInput();
            

        }
        
        private void onConfirmGameOver()
        {
            AudioManager.Instance.PlayRandomSFX(confirmGameOverSound);
            _input.DisableAllInput();
            _animator.Play(exitStateID);
            SceneLoader.Instance.LoadScoringScene();
        }
        

        void EnableGameOverInput()
        {
            _input.EnableGameOverInput();
        }
    }
}
