using UnityEngine;

namespace OduLib.Systems.Characters
{
    public class CharacterMovement : MonoBehaviour
    {
        protected Vector2 _movementDirection;

        public void MoveInputPerformed(Vector2 direction)
        {
            _movementDirection = direction;
        }

        public void MoveInputCanceled()
        {
            _movementDirection = Vector2.zero;
        }
    }
}