using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class EmergencyButtonSystem : MonoBehaviour
    {
        public bool IsPressed { get; private set; }

        public void Initialize(bool startsPressed)
        {
            IsPressed = startsPressed;
        }

        public void SetPressed(bool value)
        {
            IsPressed = value;
        }
    }
}
