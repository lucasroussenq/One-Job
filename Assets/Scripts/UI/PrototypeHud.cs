using OneJob.Player;
using UnityEngine;

namespace OneJob.UI
{
    [RequireComponent(typeof(PlayerInteractor))]
    public sealed class PrototypeHud : MonoBehaviour
    {
        private PlayerInteractor interactor;
        private GUIStyle promptStyle;
        private GUIStyle helpStyle;

        private void Awake()
        {
            interactor = GetComponent<PlayerInteractor>();
        }

        private void OnGUI()
        {
            EnsureStyles();

            float centerX = Screen.width * 0.5f;
            float centerY = Screen.height * 0.5f;
            GUI.Label(new Rect(centerX - 10f, centerY - 14f, 20f, 28f), "+", promptStyle);

            if (!string.IsNullOrEmpty(interactor.CurrentPrompt))
            {
                GUI.Label(
                    new Rect(centerX - 220f, centerY + 28f, 440f, 40f),
                    $"[E] {interactor.CurrentPrompt}",
                    promptStyle);
            }

            GUI.Label(new Rect(16f, 16f, 420f, 28f), "WASD move | Mouse look | E interact | Esc cursor", helpStyle);
        }

        private void EnsureStyles()
        {
            if (promptStyle != null)
            {
                return;
            }

            promptStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };

            helpStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                normal = { textColor = Color.white }
            };
        }
    }
}
