using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OduLib.Systems.Characters
{
    public class CharacterInputReader : MonoBehaviour
    {
        public CharacterMovement CurrentCharacterMovement;

        public Action<Vector2> MovePerformed;
        public Action MoveCanceled;

        public Action ChangeTypePerformed;

        public Action JumpPerformed;

        private ThirdPersonMovement inputActions;

        private void OnEnable()
        {
            inputActions = new ThirdPersonMovement();

            inputActions.Movement.Move.performed += InputPerform;
            inputActions.Movement.Move.canceled += InputCancel;
            inputActions.Movement.Change.performed += Change;
            inputActions.Movement.Jump.performed += Jump;

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Movement.Move.performed -= InputPerform;
            inputActions.Movement.Move.canceled -= InputCancel;
            inputActions.Movement.Change.performed -= Change;
            inputActions.Movement.Jump.performed -= Jump;

            inputActions.Disable();
        }

        private void Change(InputAction.CallbackContext context)
        {
            ChangeTypePerformed?.Invoke();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            JumpPerformed?.Invoke();
            CurrentCharacterMovement.JumpInputPerformed();
        }

        private void InputPerform(InputAction.CallbackContext callbackContext)
        {
            var direction = callbackContext.ReadValue<Vector2>();

            MovePerformed?.Invoke(direction);
            CurrentCharacterMovement.MoveInputPerformed(direction);
        }

        private void InputCancel(InputAction.CallbackContext callbackContext)
        {
            MoveCanceled?.Invoke();
            CurrentCharacterMovement.MoveInputCanceled();
        }
    }
}
