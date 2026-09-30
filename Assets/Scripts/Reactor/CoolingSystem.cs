using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class CoolingSystem : MonoBehaviour
    {
        public bool IsOn { get; private set; }

        public void Initialize(bool startsOn)
        {
            IsOn = startsOn;
        }

        public void SetCooling(bool value)
        {
            IsOn = value;
        }
    }
}
