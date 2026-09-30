using OneJob.Interaction;
using OneJob.Player;
using OneJob.UI;
using UnityEngine;

namespace OneJob.Scenarios.ReactorRoom
{
    [DisallowMultipleComponent]
    public sealed class ReactorRoomBootstrap : MonoBehaviour
    {
        private static readonly Color FloorColor = new Color(0.18f, 0.22f, 0.25f);
        private static readonly Color WallColor = new Color(0.33f, 0.38f, 0.41f);
        private static readonly Color ConsoleColor = new Color(0.12f, 0.22f, 0.28f);
        private static readonly Color SteelColor = new Color(0.46f, 0.52f, 0.55f);
        private static readonly Color ReactorColor = new Color(0.1f, 0.55f, 0.65f);
        private static readonly Color WarningColor = new Color(0.95f, 0.18f, 0.05f);

        [SerializeField] private bool buildOnAwake = true;

        private bool hasBuilt;
        private GrayboxFactory factory;

        private void Awake()
        {
            if (buildOnAwake)
            {
                Build();
            }
        }

        [ContextMenu("Build Reactor Room Graybox")]
        public void Build()
        {
            if (hasBuilt)
            {
                return;
            }

            hasBuilt = true;
            Transform generatedRoot = new GameObject("Generated Reactor Room").transform;
            generatedRoot.SetParent(transform, false);
            factory = new GrayboxFactory(generatedRoot);

            BuildShell();
            BuildLighting();
            BuildReactor();
            BuildControlStations();
            BuildValves();
            BuildDoor();
            BuildSpawnPoints();
            BuildPlayer();
        }

        private void BuildShell()
        {
            factory.CreatePrimitive(PrimitiveType.Cube, "Floor", new Vector3(0f, -0.25f, 0f), new Vector3(20f, 0.5f, 16f), FloorColor);
            factory.CreatePrimitive(PrimitiveType.Cube, "North Wall", new Vector3(0f, 3f, -8f), new Vector3(20f, 6f, 0.4f), WallColor);
            factory.CreatePrimitive(PrimitiveType.Cube, "South Wall", new Vector3(0f, 3f, 8f), new Vector3(20f, 6f, 0.4f), WallColor);
            factory.CreatePrimitive(PrimitiveType.Cube, "West Wall", new Vector3(-10f, 3f, 0f), new Vector3(0.4f, 6f, 16f), WallColor);
            factory.CreatePrimitive(PrimitiveType.Cube, "East Wall A", new Vector3(10f, 3f, -5.5f), new Vector3(0.4f, 6f, 5f), WallColor);
            factory.CreatePrimitive(PrimitiveType.Cube, "East Wall B", new Vector3(10f, 3f, 5.5f), new Vector3(0.4f, 6f, 5f), WallColor);
            factory.CreateLabel("REACTOR TRAINING ROOM\nABSOLUTELY SAFE", new Vector3(0f, 4.6f, -7.72f), 0.16f);
        }

        private void BuildLighting()
        {
            RenderSettings.ambientLight = new Color(0.28f, 0.31f, 0.34f);

            Transform sunTransform = factory.CreateEmpty("Main Directional Light", new Vector3(0f, 4f, 0f));
            sunTransform.rotation = Quaternion.Euler(50f, -30f, 0f);
            Light sun = sunTransform.gameObject.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.1f;
            sun.shadows = LightShadows.Soft;

            factory.CreatePointLight("Ceiling Light A", new Vector3(-5f, 4.8f, 1f), Color.white, 10f, 2.2f);
            factory.CreatePointLight("Ceiling Light B", new Vector3(5f, 4.8f, 1f), Color.white, 10f, 2.2f);
        }

        private void BuildReactor()
        {
            factory.CreatePrimitive(PrimitiveType.Cylinder, "Reactor Core", new Vector3(0f, 1.7f, 0f), new Vector3(2.2f, 1.7f, 2.2f), ReactorColor);
            factory.CreatePrimitive(PrimitiveType.Cylinder, "Reactor Base", new Vector3(0f, 0.25f, 0f), new Vector3(3f, 0.25f, 3f), SteelColor);
            factory.CreatePrimitive(PrimitiveType.Cylinder, "Reactor Top", new Vector3(0f, 3.35f, 0f), new Vector3(2.6f, 0.2f, 2.6f), SteelColor);
            factory.CreateLabel("REACTOR", new Vector3(0f, 4.15f, 0f), 0.18f);

            GameObject alarm = factory.CreatePrimitive(PrimitiveType.Sphere, "Alarm Beacon", new Vector3(0f, 5f, -6.9f), Vector3.one * 0.45f, WarningColor);
            alarm.GetComponent<Collider>().enabled = false;
            factory.CreatePointLight("Alarm Light", alarm.transform.position, WarningColor, 5f, 1.5f);
        }

        private void BuildControlStations()
        {
            BuildLeverStation("COOLING", new Vector3(-6f, 1.1f, -6.7f));
            BuildStatusStation("PRESSURE", new Vector3(-2f, 1.1f, -6.7f), new Color(1f, 0.65f, 0.05f));
            BuildStatusStation("TEMPERATURE", new Vector3(2f, 1.1f, -6.7f), new Color(0.15f, 0.85f, 0.3f));
            BuildEmergencyStation(new Vector3(6f, 1.1f, -6.7f));
        }

        private void BuildLeverStation(string label, Vector3 position)
        {
            Transform station = factory.CreateEmpty($"{label} Station", position);
            factory.CreatePrimitive(PrimitiveType.Cube, "Console", Vector3.zero, new Vector3(2.6f, 1.8f, 0.8f), ConsoleColor, station);
            factory.CreateLabel(label, new Vector3(0f, 1.2f, 0.45f), 0.11f, station);
            GameObject indicator = factory.CreatePrimitive(PrimitiveType.Sphere, "Status Light", new Vector3(0.65f, 0.35f, 0.47f), Vector3.one * 0.2f, WarningColor, station);
            GameObject handle = factory.CreatePrimitive(PrimitiveType.Cube, "Lever Handle", new Vector3(-0.35f, 0.15f, 0.65f), new Vector3(0.18f, 0.8f, 0.18f), SteelColor, station);

            LeverInteractable lever = station.gameObject.AddComponent<LeverInteractable>();
            lever.ConfigurePresentation(handle.transform, indicator.GetComponent<Renderer>());
            lever.Configure("Turn cooling ON", "Turn cooling OFF", false);
        }

        private void BuildStatusStation(string label, Vector3 position, Color statusColor)
        {
            Transform station = factory.CreateEmpty($"{label} Station", position);
            factory.CreatePrimitive(PrimitiveType.Cube, "Console", Vector3.zero, new Vector3(2.6f, 1.8f, 0.8f), ConsoleColor, station);
            factory.CreateLabel(label, new Vector3(0f, 1.2f, 0.45f), 0.11f, station);
            factory.CreatePrimitive(PrimitiveType.Sphere, "Status Light", new Vector3(0f, 0.2f, 0.47f), Vector3.one * 0.3f, statusColor, station);
        }

        private void BuildEmergencyStation(Vector3 position)
        {
            Transform station = factory.CreateEmpty("Emergency Station", position);
            factory.CreatePrimitive(PrimitiveType.Cube, "Console", Vector3.zero, new Vector3(2.6f, 1.8f, 0.8f), ConsoleColor, station);
            factory.CreateLabel("EMERGENCY\nDO NOT PRESS", new Vector3(0f, 1.25f, 0.45f), 0.1f, station);
            GameObject cap = factory.CreatePrimitive(PrimitiveType.Cylinder, "Emergency Button", new Vector3(0f, 0f, 0.65f), new Vector3(0.48f, 0.16f, 0.48f), WarningColor, station);
            cap.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            ButtonInteractable button = station.gameObject.AddComponent<ButtonInteractable>();
            button.ConfigurePresentation(cap.transform, cap.GetComponent<Renderer>());
            button.Configure("Press EMERGENCY", "Reset EMERGENCY", false);
        }

        private void BuildValves()
        {
            BuildValve("VALVE A", new Vector3(-4.2f, 1.1f, 1.2f), false);
            BuildValve("VALVE B", new Vector3(4.2f, 1.1f, 1.2f), false);
        }

        private void BuildValve(string label, Vector3 position, bool startsOpen)
        {
            Transform assembly = factory.CreateEmpty(label, position);
            GameObject pipe = factory.CreatePrimitive(PrimitiveType.Cylinder, "Pipe", Vector3.zero, new Vector3(0.35f, 1.3f, 0.35f), SteelColor, assembly);
            pipe.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            GameObject wheel = factory.CreatePrimitive(PrimitiveType.Cylinder, "Valve Wheel", new Vector3(0f, 0.8f, 0f), new Vector3(0.85f, 0.14f, 0.85f), WarningColor, assembly);
            wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject indicator = factory.CreatePrimitive(PrimitiveType.Sphere, "Valve Status", new Vector3(0f, 1.55f, 0f), Vector3.one * 0.2f, WarningColor, assembly);
            factory.CreateLabel(label, new Vector3(0f, 1.95f, 0f), 0.12f, assembly);

            ValveInteractable valve = assembly.gameObject.AddComponent<ValveInteractable>();
            valve.ConfigurePresentation(wheel.transform, indicator.GetComponent<Renderer>());
            valve.Configure($"Open {label}", $"Close {label}", startsOpen);
        }

        private void BuildDoor()
        {
            Transform pivot = factory.CreateEmpty("Exit Door Pivot", new Vector3(9.75f, 0f, -1.5f));
            GameObject door = factory.CreatePrimitive(PrimitiveType.Cube, "Exit Door", new Vector3(0f, 1.5f, 1.5f), new Vector3(0.25f, 3f, 3f), new Color(0.75f, 0.65f, 0.12f), pivot);
            factory.CreateLabel("EXIT", new Vector3(-0.2f, 3.5f, 1.5f), 0.13f, pivot).transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

            DoorInteractable doorInteractable = pivot.gameObject.AddComponent<DoorInteractable>();
            doorInteractable.ConfigurePresentation(pivot);
            doorInteractable.Configure("Open exit door", "Close exit door", false);
            door.name = "Exit Door Panel";
        }

        private void BuildSpawnPoints()
        {
            float[] xPositions = { -6f, -2f, 2f, 6f };
            for (int index = 0; index < xPositions.Length; index++)
            {
                Vector3 position = new Vector3(xPositions[index], 0.03f, 5.5f);
                GameObject marker = factory.CreatePrimitive(
                    PrimitiveType.Cylinder,
                    $"Spawn Point {index + 1}",
                    position,
                    new Vector3(0.65f, 0.03f, 0.65f),
                    new Color(0.15f, 0.55f, 0.8f));
                marker.GetComponent<Collider>().enabled = false;
                factory.CreateLabel($"P{index + 1}", position + Vector3.up * 0.08f, 0.1f);
            }
        }

        private void BuildPlayer()
        {
            GameObject player = new GameObject("Local Prototype Player");
            player.transform.SetParent(transform, false);
            player.transform.position = new Vector3(-6f, 0.05f, 5.5f);
            player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.35f;
            controller.center = new Vector3(0f, 0.9f, 0f);
            controller.stepOffset = 0.3f;

            player.AddComponent<PlayerInputReader>();
            player.AddComponent<PlayerMotor>();

            GameObject cameraObject = new GameObject("Player Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 1.62f, 0f);
            Camera playerCamera = cameraObject.AddComponent<Camera>();
            playerCamera.fieldOfView = 75f;
            playerCamera.nearClipPlane = 0.05f;
            cameraObject.AddComponent<AudioListener>();

            PlayerLook look = player.AddComponent<PlayerLook>();
            look.SetCameraPivot(cameraObject.transform);
            PlayerInteractor interactor = player.AddComponent<PlayerInteractor>();
            interactor.SetViewCamera(playerCamera);
            player.AddComponent<PrototypeHud>();
        }
    }
}
