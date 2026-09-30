using UnityEngine;

namespace OneJob.Interaction
{
    public sealed class ButtonInteractable : StatefulInteractable
    {
        [SerializeField] private Transform buttonCap;
        [SerializeField] private Renderer indicator;
        [SerializeField, Min(0f)] private float pressDepth = 0.08f;
        [SerializeField] private Color inactiveColor = new Color(0.45f, 0.05f, 0.05f);
        [SerializeField] private Color activeColor = new Color(1f, 0.1f, 0.05f);

        private Vector3 releasedPosition;

        public void ConfigurePresentation(Transform cap, Renderer stateIndicator)
        {
            buttonCap = cap;
            indicator = stateIndicator;
            releasedPosition = buttonCap != null ? buttonCap.localPosition : Vector3.zero;
            ApplyVisualState(IsActive);
        }

        protected override void Awake()
        {
            if (buttonCap != null)
            {
                releasedPosition = buttonCap.localPosition;
            }

            base.Awake();
        }

        protected override void ApplyVisualState(bool active)
        {
            if (buttonCap != null)
            {
                buttonCap.localPosition = releasedPosition + Vector3.back * (active ? pressDepth : 0f);
            }

            SetRendererColor(indicator, active ? activeColor : inactiveColor);
        }
    }
}
