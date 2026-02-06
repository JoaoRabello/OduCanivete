using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OduLib.Systems.Characters.ThreeDimensional
{
    public class CharacterInputReader : MonoBehaviour
    {
        public CharacterMovement CurrentCharacterMovement;

        public Action<Vector2> MovePerformed;
        public Action MoveCanceled;

        private ThirdPersonMovement inputActions;

        private void OnEnable()
        {
            inputActions = new ThirdPersonMovement();

            inputActions.Movement.Move.performed += InputPerform;
            inputActions.Movement.Move.canceled += InputCancel;

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Movement.Move.performed -= InputPerform;
            inputActions.Movement.Move.canceled -= InputCancel;

            inputActions.Disable();
        }

        private void InputPerform(InputAction.CallbackContext callbackContext)
        {
            CurrentCharacterMovement.MoveInputPerformed(callbackContext.ReadValue<Vector2>());
        }

        private void InputCancel(InputAction.CallbackContext callbackContext)
        {
            CurrentCharacterMovement.MoveInputCanceled();
        }
    }
}
