using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class TemperatureSystem : MonoBehaviour
    {
        private ReactorSimulationSettings settings;

        public float CurrentTemperature { get; private set; }

        public ReactorState Severity
        {
            get
            {
                if (CurrentTemperature >= settings.temperatureCriticalThreshold)
                {
                    return ReactorState.Critical;
                }

                return CurrentTemperature >= settings.temperatureWarningThreshold
                    ? ReactorState.Warning
                    : ReactorState.Normal;
            }
        }

        public void Initialize(ReactorSimulationSettings simulationSettings)
        {
            settings = simulationSettings;
            CurrentTemperature = Mathf.Clamp(
                settings.initialTemperature,
                settings.minimumTemperature,
                settings.maximumTemperature);
        }

        public void Tick(bool coolingOn, float deltaTime)
        {
            float rate = coolingOn
                ? -settings.coolingRatePerSecond
                : settings.heatingRatePerSecond;

            CurrentTemperature = Mathf.Clamp(
                CurrentTemperature + rate * deltaTime,
                settings.minimumTemperature,
                settings.maximumTemperature);
        }
    }
}
