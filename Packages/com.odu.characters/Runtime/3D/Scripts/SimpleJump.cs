using UnityEngine;

namespace OduLib.Systems.Characters
{
    public class SimpleJump : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Transform _jumpRaycastOrigin;
        [SerializeField] private float _jumpForce;

        private bool _wannaJump;
        private bool _isGrounded;

        public void JumpInputPerformed()
        {
            _wannaJump = true;
        }

        private void Update()
        {
            JumpBehaviour();
        }

        private void JumpBehaviour()
        {
            _isGrounded = Physics.Raycast(_jumpRaycastOrigin.position, Vector3.down, 0.2f, _groundLayerMask);

            if (_isGrounded && _wannaJump)
            {
                Jump();
            }
            _wannaJump = false;
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * 100 * _jumpForce);
        }
    }
}