using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace TonyLearning.ShootingGame.Input
{
[CreateAssetMenu(menuName = ("Player Input"), fileName = (""))]
    public class PlayerInput : ScriptableObject, InputActions.IGameplayActions, InputActions.IPauseMenuActions, InputActions.IGameOverActions
    {
        public event UnityAction<Vector2> onMove = delegate(Vector2 arg0) {  };
        public event UnityAction onStopMove = delegate {  };
        
        public event UnityAction onFire = delegate {  };
        public event UnityAction onStopFire = delegate {  }; 
        
        public event UnityAction onDodge = delegate {  };
        
        public event UnityAction onOverDrive = delegate {  }; 

        public event UnityAction onPause = delegate {  };
        public event UnityAction onUnPause = delegate {  };
        
        public event UnityAction onLaunchMissile = delegate {  };
        public event UnityAction onConfirmGameOver = delegate {  };
        
        private InputActions _inputActions;

        private void OnEnable()
        {
            _inputActions = new InputActions();
            
            _inputActions.Gameplay.SetCallbacks(this);
            _inputActions.PauseMenu.SetCallbacks(this);
            _inputActions.GameOver.SetCallbacks(this);
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        private void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
        {
            _inputActions.Disable();
            actionMap.Enable();

            if (isUIInput)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void SwitchToDynamicUpdateMode()
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        }
        
        public void SwitchToFixedUpdateMode()
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        }

        public void EnableGameplayInput()
        {
            SwitchActionMap(_inputActions.Gameplay, false);
        }

        public void EnablePauseMenuInput()
        {
            SwitchActionMap(_inputActions.PauseMenu, true);
        }

        public void EnableGameOverInput() => SwitchActionMap(_inputActions.GameOver, false);
        
        // public void EnableGameplayInput()
        // {
        //     _inputActions.Gameplay.Enable();
        //
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        public void DisableAllInput()
        {
            // _inputActions.Gameplay.Disable();
            // _inputActions.PauseMenu.Disable();
            _inputActions.Disable();
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

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onPause?.Invoke();
            }
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onUnPause?.Invoke();
            }
        }
        
        public void OnLaunchMissile(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onLaunchMissile?.Invoke();
            }
        }

        public void OnConfirmGameOver(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onConfirmGameOver?.Invoke();
            }
        }
    }
}
