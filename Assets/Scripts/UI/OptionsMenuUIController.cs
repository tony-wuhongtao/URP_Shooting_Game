using System;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class OptionsMenuUIController : MonoBehaviour
    {
        [SerializeField] private Canvas optionsCanvas;
        [SerializeField] private Button buttonMainMenu;


        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            GameManager.GameState = GameState.Options;
            UIInput.Instance.SelectUI(buttonMainMenu);
            
            
        }

        private void Awake()
        {
            ButtonPressedBehaviour.buttonFuntionTable.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        }

        private void OnDisable()
        {
            ButtonPressedBehaviour.buttonFuntionTable.Clear();
        }

        private void OnButtonMainMenuClicked()
        {
            optionsCanvas.enabled = false;
            SceneLoader.Instance.LoadMainMainScene();
        }
    }
}
