using OneJob.Reactor;
using UnityEngine;

namespace OneJob.UI
{
    public sealed class ReactorStatusHud : MonoBehaviour
    {
        [SerializeField] private ReactorController reactor;

        private GUIStyle panelStyle;
        private GUIStyle valueStyle;
        private GUIStyle resultStyle;

        public void SetReactor(ReactorController value)
        {
            reactor = value;
        }

        private void OnGUI()
        {
            if (reactor == null)
            {
                return;
            }

            EnsureStyles();

            GUI.Box(new Rect(Screen.width - 270f, 16f, 254f, 154f), GUIContent.none, panelStyle);
            GUI.Label(new Rect(Screen.width - 250f, 28f, 220f, 24f), $"Temperature: {reactor.Temperature:0.0} °C", valueStyle);
            GUI.Label(new Rect(Screen.width - 250f, 56f, 220f, 24f), $"Pressure: {reactor.Pressure:0.0}%", valueStyle);
            GUI.Label(new Rect(Screen.width - 250f, 84f, 220f, 24f), $"Cooling: {(reactor.CoolingOn ? "ON" : "OFF")}", valueStyle);
            GUI.Label(new Rect(Screen.width - 250f, 112f, 220f, 24f), $"Reactor: {reactor.State.ToString().ToUpperInvariant()}", valueStyle);
            GUI.Label(new Rect(Screen.width - 250f, 140f, 220f, 24f), $"Time: {FormatTime(reactor.RemainingSeconds)}", valueStyle);

            if (reactor.Result != RoundResult.InProgress)
            {
                string result = reactor.Result == RoundResult.Success ? "SUCCESS" : "FAILURE";
                resultStyle.normal.textColor = reactor.Result == RoundResult.Success
                    ? new Color(0.2f, 1f, 0.35f)
                    : new Color(1f, 0.2f, 0.1f);
                GUI.Label(new Rect(0f, Screen.height * 0.32f, Screen.width, 100f), result, resultStyle);
            }
        }

        private void EnsureStyles()
        {
            if (panelStyle != null)
            {
                return;
            }

            panelStyle = new GUIStyle(GUI.skin.box);
            valueStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 17,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            resultStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 48,
                fontStyle = FontStyle.Bold
            };
        }

        private static string FormatTime(float seconds)
        {
            int totalSeconds = Mathf.CeilToInt(seconds);
            return $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
        }
    }
}
