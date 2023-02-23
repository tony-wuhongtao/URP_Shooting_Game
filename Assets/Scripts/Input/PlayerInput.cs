using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TonyLearning.ShootingGame.Input
{
[CreateAssetMenu(menuName = ("Player Input"), fileName = (""))]
    public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
    {
        public event UnityAction<Vector2> onMove = delegate(Vector2 arg0) {  };
        public event UnityAction onStopMove = delegate {  };
        
        public event UnityAction onFire = delegate {  };
        public event UnityAction onStopFire = delegate {  }; 
        
        public event UnityAction onDodge = delegate {  };
        
        public event UnityAction onOverDrive = delegate {  }; 

        private InputActions _inputActions;

        private void OnEnable()
        {
            _inputActions = new InputActions();
            
            _inputActions.Gameplay.SetCallbacks(this);
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void EnableGameplayInput()
        {
            _inputActions.Gameplay.Enable();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void DisableAllInput()
        {
            _inputActions.Gameplay.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                onStopMove?.Invoke();
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onFire?.Invoke();
            }

            if (context.canceled)
            {
                onStopFire?.Invoke();
            }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onDodge?.Invoke();
            }
        }

        public void OnOverdrive(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onOverDrive?.Invoke();
            }
        }
    }
}
