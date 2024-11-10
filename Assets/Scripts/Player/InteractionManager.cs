using Interactables;
using UnityEngine;

namespace Player
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private LayerMask interactionLayer;

        private PlayerManager _player;

        private InteractableBase _interactable;
        private Outline _lastOutline;
    
        public void Init(PlayerManager playerManager)
        {
            _player = playerManager;
        }

        public void Update()
        {
            var cam = _player.PlayerCamera;
            if (cam == null) return;

            if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit,
                interactionDistance,
                interactionLayer))
            {
                if (_lastOutline != null)
                {
                    _lastOutline.ToggleOutline(false);
                    _lastOutline = null;
                }

                _interactable = null;
                PlayerUIManager.Instance.ToggleInteractablePopup(false);
                return;
            }
        
            //if (!hit.collider.CompareTag("Interactable")) return;
            _interactable = hit.collider.GetComponent<InteractableBase>();
            if (_interactable == null)
            {
                //Check if Collider is on a Model but not the main Interactable Object;
                _interactable = hit.collider.transform.parent.GetComponent<InteractableBase>();
                if (_interactable == null) return;
            }

            PlayerUIManager.Instance.ToggleInteractablePopup(true);

            if (_lastOutline == _interactable.Outline) return;

            _lastOutline = _interactable.Outline;
            _lastOutline.ToggleOutline();

        }

        public void TryInteract()
        {
            if (_interactable == null) return;
            _interactable.Interact(_player);
        }
    }
}
