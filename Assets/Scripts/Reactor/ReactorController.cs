using System;
using UnityEngine;

namespace OneJob.Reactor
{
    public sealed class ReactorController : MonoBehaviour
    {
        [SerializeField] private bool startOnStart = true;
        [SerializeField] private ReactorSimulationSettings settings = new ReactorSimulationSettings();
        [SerializeField] private TemperatureSystem temperature;
        [SerializeField] private PressureSystem pressure;
        [SerializeField] private CoolingSystem cooling;
        [SerializeField] private ValveSystem valveA;
        [SerializeField] private ValveSystem valveB;
        [SerializeField] private EmergencyButtonSystem emergencyButton;

        public ReactorState State { get; private set; } = ReactorState.Normal;

        public RoundResult Result { get; private set; } = RoundResult.InProgress;

        public bool IsRoundRunning { get; private set; }

        public float ElapsedSeconds { get; private set; }

        public float RemainingSeconds => Mathf.Max(0f, settings.roundDurationSeconds - ElapsedSeconds);

        public float CriticalSeconds { get; private set; }

        public float Temperature => temperature != null ? temperature.CurrentTemperature : 0f;

        public float Pressure => pressure != null ? pressure.CurrentPressure : 0f;

        public bool CoolingOn => cooling != null && cooling.IsOn;

        public event Action<ReactorState> StateChanged;

        public event Action<RoundResult> RoundEnded;

        public void Configure(
            ReactorSimulationSettings simulationSettings,
            TemperatureSystem temperatureSystem,
            PressureSystem pressureSystem,
            CoolingSystem coolingSystem,
            ValveSystem valveASystem,
            ValveSystem valveBSystem,
            EmergencyButtonSystem emergencySystem)
        {
            settings = simulationSettings;
            temperature = temperatureSystem;
            pressure = pressureSystem;
            cooling = coolingSystem;
            valveA = valveASystem;
            valveB = valveBSystem;
            emergencyButton = emergencySystem;
        }

        private void Start()
        {
            if (startOnStart)
            {
                StartRound();
            }
        }

        private void Update()
        {
            if (!IsRoundRunning)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            ElapsedSeconds += deltaTime;
            temperature.Tick(cooling.IsOn, deltaTime);
            pressure.Tick(valveB.IsOpen, deltaTime);

            SetState(EvaluateState());

            if (State == ReactorState.Critical)
            {
                CriticalSeconds += deltaTime;
                if (CriticalSeconds >= settings.criticalGraceSeconds)
                {
                    Explode();
                    return;
                }
            }
            else
            {
                CriticalSeconds = 0f;
            }

            if (ElapsedSeconds >= settings.roundDurationSeconds)
            {
                CompleteRound(RoundResult.Success);
            }
        }

        public void StartRound()
        {
            if (!HasRequiredSystems())
            {
                Debug.LogError("ReactorController requires Temperature, Pressure, Cooling, both Valves, and Emergency Button systems.", this);
                return;
            }

            settings ??= new ReactorSimulationSettings();
            temperature.Initialize(settings);
            pressure.Initialize(settings);
            cooling.Initialize(settings.coolingStartsOn);
            valveA.Initialize("Valve A", settings.valveAStartsOpen);
            valveB.Initialize("Valve B", settings.valveBStartsOpen);
            emergencyButton.Initialize(settings.emergencyStartsPressed);

            ElapsedSeconds = 0f;
            CriticalSeconds = 0f;
            Result = RoundResult.InProgress;
            IsRoundRunning = true;
            SetState(ReactorState.Normal, true);
        }

        private ReactorState EvaluateState()
        {
            if (emergencyButton.IsPressed)
            {
                return ReactorState.Critical;
            }

            return (ReactorState)Mathf.Max((int)temperature.Severity, (int)pressure.Severity);
        }

        private void Explode()
        {
            SetState(ReactorState.Exploded);
            CompleteRound(RoundResult.Failure);
            Debug.LogWarning("Reactor exploded. Round failed.", this);
        }

        private void CompleteRound(RoundResult result)
        {
            IsRoundRunning = false;
            Result = result;
            RoundEnded?.Invoke(result);
        }

        private void SetState(ReactorState nextState, bool force = false)
        {
            if (!force && State == nextState)
            {
                return;
            }

            State = nextState;
            StateChanged?.Invoke(nextState);
        }

        private bool HasRequiredSystems()
        {
            return temperature != null
                && pressure != null
                && cooling != null
                && valveA != null
                && valveB != null
                && emergencyButton != null;
        }
    }
}
