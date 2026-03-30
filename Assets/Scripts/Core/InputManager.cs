using UnityEngine;
using UnityEngine.InputSystem;
using DungTran31.Utilities;

namespace DungTran31.Core
{
    // This script acts as a single point for all other scripts to get
    // the current input from. It uses Unity's new Input System and
    // functions should be mapped to their corresponding controls
    // using a PlayerInput component with Unity Events.

    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : Singleton<InputManager>
    {
        private Vector2 moveDirection = Vector2.zero;
        private bool interactPressed = false;
        private bool submitPressed = false;

        public void MovePressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                moveDirection = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                moveDirection = context.ReadValue<Vector2>();
            }
        }

        public void InteractButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                interactPressed = true;
            }
            else if (context.canceled)
            {
                interactPressed = false;
            }
        }

        public void SubmitPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                submitPressed = true;
            }
            else if (context.canceled)
            {
                submitPressed = false;
            }
        }

        // for any of the below 'Get' methods, if we're getting it then we're also using it,
        // which means we should set it to false so that it can't be used again until actually
        // pressed again.

        public bool GetInteractPressed()
        {
            bool result = interactPressed;
            interactPressed = false;
            return result;
        }

        public bool GetSubmitPressed()
        {
            bool result = submitPressed;
            submitPressed = false;
            return result;
        }

        public void RegisterSubmitPressed()
        {
            submitPressed = false;
        }

    }

}
