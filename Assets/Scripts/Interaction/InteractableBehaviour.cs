using UnityEngine;

namespace OneJob.Interaction
{
    public abstract class InteractableBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool interactionEnabled = true;

        public abstract string InteractionPrompt { get; }

        public virtual bool CanInteract(GameObject interactor)
        {
            return interactionEnabled && isActiveAndEnabled && interactor != null;
        }

        public abstract void Interact(GameObject interactor);

        public void SetInteractionEnabled(bool value)
        {
            interactionEnabled = value;
        }
    }
}
