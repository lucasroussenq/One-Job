using UnityEngine;

namespace OneJob.Interaction
{
    public sealed class ValveInteractable : StatefulInteractable
    {
        [SerializeField] private Transform wheel;
        [SerializeField] private Renderer indicator;
        [SerializeField] private float openAngle = 120f;
        [SerializeField] private Color closedColor = new Color(0.7f, 0.12f, 0.08f);
        [SerializeField] private Color openColor = new Color(0.1f, 0.75f, 0.35f);

        private Quaternion closedRotation = Quaternion.identity;

        public void ConfigurePresentation(Transform valveWheel, Renderer stateIndicator)
        {
            wheel = valveWheel;
            closedRotation = wheel != null ? wheel.localRotation : Quaternion.identity;
            indicator = stateIndicator;
            ApplyVisualState(IsActive);
        }

        protected override void Awake()
        {
            if (wheel != null)
            {
                closedRotation = wheel.localRotation;
            }

            base.Awake();
        }

        protected override void ApplyVisualState(bool active)
        {
            if (wheel != null)
            {
                wheel.localRotation = closedRotation * Quaternion.AngleAxis(active ? openAngle : 0f, Vector3.forward);
            }

            SetRendererColor(indicator, active ? openColor : closedColor);
        }
    }
}
