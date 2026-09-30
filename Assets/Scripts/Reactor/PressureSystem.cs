using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class PressureSystem : MonoBehaviour
    {
        private ReactorSimulationSettings settings;

        public float CurrentPressure { get; private set; }

        public ReactorState Severity
        {
            get
            {
                if (CurrentPressure >= settings.pressureCriticalThreshold)
                {
                    return ReactorState.Critical;
                }

                return CurrentPressure >= settings.pressureWarningThreshold
                    ? ReactorState.Warning
                    : ReactorState.Normal;
            }
        }

        public void Initialize(ReactorSimulationSettings simulationSettings)
        {
            settings = simulationSettings;
            CurrentPressure = Mathf.Clamp(
                settings.initialPressure,
                settings.minimumPressure,
                settings.maximumPressure);
        }

        public void Tick(bool valveBOpen, float deltaTime)
        {
            float rate = valveBOpen
                ? -settings.openValvePressureRatePerSecond
                : settings.closedValvePressureRatePerSecond;

            CurrentPressure = Mathf.Clamp(
                CurrentPressure + rate * deltaTime,
                settings.minimumPressure,
                settings.maximumPressure);
        }
    }
}
