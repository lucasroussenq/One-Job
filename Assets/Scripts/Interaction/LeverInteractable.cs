using UnityEngine;

namespace OneJob.Interaction
{
    public sealed class LeverInteractable : StatefulInteractable
    {
        [SerializeField] private Transform handle;
        [SerializeField] private Renderer indicator;
        [SerializeField] private float offAngle = 45f;
        [SerializeField] private float onAngle = -45f;
        [SerializeField] private Color inactiveColor = new Color(0.5f, 0.08f, 0.05f);
        [SerializeField] private Color activeColor = new Color(0.1f, 0.9f, 0.25f);

        public void ConfigurePresentation(Transform leverHandle, Renderer stateIndicator)
        {
            handle = leverHandle;
            indicator = stateIndicator;
            ApplyVisualState(IsActive);
        }

        protected override void ApplyVisualState(bool active)
        {
            if (handle != null)
            {
                handle.localRotation = Quaternion.Euler(active ? onAngle : offAngle, 0f, 0f);
            }

            SetRendererColor(indicator, active ? activeColor : inactiveColor);
        }
    }
}
