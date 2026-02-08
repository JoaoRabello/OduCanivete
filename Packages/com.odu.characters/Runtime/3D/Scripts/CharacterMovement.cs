using UnityEngine;

namespace OduLib.Systems.Characters
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] protected SimpleJump _jump;
        protected Vector2 _movementDirection;

        public virtual void Enter()
        {
            _movementDirection = Vector2.zero;
        }

        public virtual void Exit()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void MoveInputPerformed(Vector2 direction)
        {
            _movementDirection = direction;
        }

        public void MoveInputCanceled()
        {
            _movementDirection = Vector2.zero;
        }

        public void JumpInputPerformed()
        {
            _jump.JumpInputPerformed();
        }
    }
}