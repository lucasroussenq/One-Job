using System;
using UnityEngine;
using UnityEngine.Events;

namespace OneJob.Interaction
{
    public abstract class StatefulInteractable : InteractableBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        [SerializeField] private bool initialState;
        [SerializeField] private string inactivePrompt = "Activate";
        [SerializeField] private string activePrompt = "Deactivate";
        [SerializeField] private UnityEvent<bool> stateChanged = new UnityEvent<bool>();

        public bool IsActive { get; private set; }

        public event Action<bool> StateChanged;

        public override string InteractionPrompt => IsActive ? activePrompt : inactivePrompt;

        public void Configure(string promptWhenInactive, string promptWhenActive, bool startsActive)
        {
            inactivePrompt = promptWhenInactive;
            activePrompt = promptWhenActive;
            initialState = startsActive;
            ApplyInitialState();
        }

        public override void Interact(GameObject interactor)
        {
            if (CanInteract(interactor))
            {
                SetState(!IsActive);
            }
        }

        public void SetState(bool value)
        {
            if (IsActive == value)
            {
                return;
            }

            IsActive = value;
            ApplyVisualState(value);
            stateChanged.Invoke(value);
            StateChanged?.Invoke(value);
        }

        protected virtual void Awake()
        {
            ApplyInitialState();
        }

        protected abstract void ApplyVisualState(bool active);

        protected static void SetRendererColor(Renderer target, Color color)
        {
            if (target == null)
            {
                return;
            }

            MaterialPropertyBlock properties = new MaterialPropertyBlock();
            target.GetPropertyBlock(properties);
            properties.SetColor(BaseColorId, color);
            properties.SetColor(ColorId, color);
            target.SetPropertyBlock(properties);
        }

        private void ApplyInitialState()
        {
            IsActive = initialState;
            ApplyVisualState(IsActive);
        }
    }
}
