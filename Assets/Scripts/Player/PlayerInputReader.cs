using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace OneJob.Player
{
    [DefaultExecutionOrder(-100)]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        public Vector2 Move { get; private set; }

        public Vector2 LookDelta { get; private set; }

        public bool InteractPressed { get; private set; }

        public bool ReleaseCursorPressed { get; private set; }

        public bool CaptureCursorPressed { get; private set; }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            ReadInputSystem();
#elif ENABLE_LEGACY_INPUT_MANAGER
            ReadLegacyInput();
#else
            Move = Vector2.zero;
            LookDelta = Vector2.zero;
            InteractPressed = false;
            ReleaseCursorPressed = false;
            CaptureCursorPressed = false;
#endif
        }

#if ENABLE_INPUT_SYSTEM
        private void ReadInputSystem()
        {
            Keyboard keyboard = Keyboard.current;
            Mouse mouse = Mouse.current;

            Move = keyboard == null
                ? Vector2.zero
                : new Vector2(
                    ReadAxis(keyboard.aKey.isPressed, keyboard.dKey.isPressed),
                    ReadAxis(keyboard.sKey.isPressed, keyboard.wKey.isPressed));

            Move = Vector2.ClampMagnitude(Move, 1f);
            LookDelta = mouse?.delta.ReadValue() ?? Vector2.zero;
            InteractPressed = keyboard?.eKey.wasPressedThisFrame ?? false;
            ReleaseCursorPressed = keyboard?.escapeKey.wasPressedThisFrame ?? false;
            CaptureCursorPressed = mouse?.leftButton.wasPressedThisFrame ?? false;
        }

        private static float ReadAxis(bool negative, bool positive)
        {
            return (positive ? 1f : 0f) - (negative ? 1f : 0f);
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void ReadLegacyInput()
        {
            Move = Vector2.ClampMagnitude(
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
                1f);
            LookDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * 15f;
            InteractPressed = Input.GetKeyDown(KeyCode.E);
            ReleaseCursorPressed = Input.GetKeyDown(KeyCode.Escape);
            CaptureCursorPressed = Input.GetMouseButtonDown(0);
        }
#endif
    }
}
