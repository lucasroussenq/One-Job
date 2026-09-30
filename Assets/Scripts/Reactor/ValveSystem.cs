using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class ValveSystem : MonoBehaviour
    {
        [SerializeField] private string valveName = "Valve";

        public string ValveName => valveName;

        public bool IsOpen { get; private set; }

        public void Initialize(string displayName, bool startsOpen)
        {
            valveName = displayName;
            IsOpen = startsOpen;
        }

        public void SetOpen(bool value)
        {
            IsOpen = value;
        }
    }
}
