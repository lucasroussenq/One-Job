using UnityEngine;

namespace OneJob.Interaction
{
    public sealed class DoorInteractable : StatefulInteractable
    {
        [SerializeField] private Transform doorPivot;
        [SerializeField] private float openAngle = 95f;

        private Quaternion closedRotation = Quaternion.identity;

        public void ConfigurePresentation(Transform pivot)
        {
            doorPivot = pivot;
            closedRotation = doorPivot != null ? doorPivot.localRotation : Quaternion.identity;
            ApplyVisualState(IsActive);
        }

        protected override void Awake()
        {
            if (doorPivot != null)
            {
                closedRotation = doorPivot.localRotation;
            }

            base.Awake();
        }

        protected override void ApplyVisualState(bool active)
        {
            if (doorPivot != null)
            {
                doorPivot.localRotation = closedRotation * Quaternion.Euler(0f, active ? openAngle : 0f, 0f);
            }
        }
    }
}
