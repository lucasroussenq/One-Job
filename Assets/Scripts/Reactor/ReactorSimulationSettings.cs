using System;
using UnityEngine;

namespace OneJob.Reactor
{
    [Serializable]
    public sealed class ReactorSimulationSettings
    {
        [Header("Round")]
        [Min(1f)] public float roundDurationSeconds = 180f;
        [Min(0.1f)] public float criticalGraceSeconds = 10f;

        [Header("Initial Controls")]
        public bool coolingStartsOn;
        public bool valveAStartsOpen;
        public bool valveBStartsOpen;
        public bool emergencyStartsPressed;

        [Header("Temperature")]
        public float initialTemperature = 45f;
        public float minimumTemperature;
        public float maximumTemperature = 140f;
        [Min(0f)] public float heatingRatePerSecond = 4f;
        [Min(0f)] public float coolingRatePerSecond = 6f;
        public float temperatureWarningThreshold = 75f;
        public float temperatureCriticalThreshold = 100f;

        [Header("Pressure")]
        public float initialPressure = 40f;
        public float minimumPressure;
        public float maximumPressure = 120f;
        [Min(0f)] public float closedValvePressureRatePerSecond = 3f;
        [Min(0f)] public float openValvePressureRatePerSecond = 5f;
        public float pressureWarningThreshold = 75f;
        public float pressureCriticalThreshold = 95f;
    }
}
