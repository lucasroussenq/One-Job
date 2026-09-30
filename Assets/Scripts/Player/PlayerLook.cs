using UnityEngine;

namespace OneJob.Player
{
    [RequireComponent(typeof(PlayerInputReader))]
    public sealed class PlayerLook : MonoBehaviour
    {
        [SerializeField] private Transform cameraPivot;
        [SerializeField, Min(0f)] private float sensitivity = 0.08f;
        [SerializeField, Range(1f, 89f)] private float pitchLimit = 85f;

        private PlayerInputReader input;
        private float pitch;

        public void SetCameraPivot(Transform value)
        {
            cameraPivot = value;
        }

        private void Awake()
        {
            input = GetComponent<PlayerInputReader>();

            if (cameraPivot == null)
            {
                Camera childCamera = GetComponentInChildren<Camera>();
                cameraPivot = childCamera != null ? childCamera.transform : null;
            }
        }

        private void OnEnable()
        {
            SetCursorCaptured(true);
        }

        private void Update()
        {
            if (input.ReleaseCursorPressed)
            {
                SetCursorCaptured(false);
            }
            else if (input.CaptureCursorPressed && Cursor.lockState != CursorLockMode.Locked)
            {
                SetCursorCaptured(true);
            }

            if (cameraPivot == null || Cursor.lockState != CursorLockMode.Locked)
            {
                return;
            }

            Vector2 look = input.LookDelta * sensitivity;
            transform.Rotate(Vector3.up, look.x, Space.Self);

            pitch = Mathf.Clamp(pitch - look.y, -pitchLimit, pitchLimit);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private static void SetCursorCaptured(bool captured)
        {
            Cursor.lockState = captured ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !captured;
        }
    }
}
