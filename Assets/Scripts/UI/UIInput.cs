using System;
using TonyLearning.ShootingGame.Input;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace TonyLearning.ShootingGame.UI
{
    public class UIInput : Singleton<UIInput>
    {
        [SerializeField] private PlayerInput _playerInput;
        private InputSystemUIInputModule UIInputModule;

      

        protected override void Awake()
        {
            base.Awake();
            UIInputModule = GetComponent<InputSystemUIInputModule>();
            UIInputModule.enabled = false;
        }

        public void SelectUI(Selectable UIObject)
        {
            UIObject.Select();
            UIObject.OnSelect(null);
            UIInputModule.enabled = true;
        }

        public void DisableAllUIInputs()
        {
            _playerInput.DisableAllInput();
            UIInputModule.enabled = false;
        }
    }
}
