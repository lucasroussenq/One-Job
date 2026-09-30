using OneJob.Interaction;
using UnityEngine;

namespace OneJob.Player
{
    [RequireComponent(typeof(PlayerInputReader))]
    public sealed class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera viewCamera;
        [SerializeField, Min(0.1f)] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactionMask = ~0;

        private PlayerInputReader input;
        private Collider cachedCollider;
        private IInteractable cachedInteractable;

        public string CurrentPrompt { get; private set; } = string.Empty;

        public void SetViewCamera(Camera value)
        {
            viewCamera = value;
        }

        private void Awake()
        {
            input = GetComponent<PlayerInputReader>();

            if (viewCamera == null)
            {
                viewCamera = GetComponentInChildren<Camera>();
            }
        }

        private void Update()
        {
            IInteractable interactable = FindFocusedInteractable();
            bool canInteract = interactable != null && interactable.CanInteract(gameObject);
            CurrentPrompt = canInteract ? interactable.InteractionPrompt : string.Empty;

            if (canInteract && input.InteractPressed)
            {
                interactable.Interact(gameObject);
            }
        }

        private IInteractable FindFocusedInteractable()
        {
            if (viewCamera == null)
            {
                ClearCache();
                return null;
            }

            Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionMask, QueryTriggerInteraction.Ignore))
            {
                ClearCache();
                return null;
            }

            if (hit.collider == cachedCollider)
            {
                return cachedInteractable;
            }

            cachedCollider = hit.collider;
            cachedInteractable = FindInParents(hit.collider);
            return cachedInteractable;
        }

        private static IInteractable FindInParents(Collider source)
        {
            MonoBehaviour[] behaviours = source.GetComponentsInParent<MonoBehaviour>(true);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is IInteractable interactable)
                {
                    return interactable;
                }
            }

            return null;
        }

        private void ClearCache()
        {
            cachedCollider = null;
            cachedInteractable = null;
        }
    }
}
