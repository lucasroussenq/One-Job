using UnityEngine;

namespace OneJob.Player
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputReader))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float moveSpeed = 5f;
        [SerializeField] private float gravity = -24f;
        [SerializeField, Min(0f)] private float groundedVelocity = 2f;

        private CharacterController characterController;
        private PlayerInputReader input;
        private float verticalVelocity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputReader>();
        }

        private void Update()
        {
            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -groundedVelocity;
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector2 moveInput = input.Move;
            Vector3 planarVelocity = transform.right * moveInput.x + transform.forward * moveInput.y;
            Vector3 velocity = planarVelocity * moveSpeed + Vector3.up * verticalVelocity;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
