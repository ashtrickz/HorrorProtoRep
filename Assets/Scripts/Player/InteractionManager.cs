using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactionLayer;

    private PlayerManager _player;

    private IInteractable _interactable;
    
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
            _interactable = null;
            PlayerUIManager.Instance.ToggleInteractablePopup(false);
            return;
        }
        
        if (!hit.collider.CompareTag("Interactable")) return;
        _interactable = hit.collider.GetComponent<IInteractable>();

        PlayerUIManager.Instance.ToggleInteractablePopup(true);
    }

    public void TryInteract()
    {
        if (_interactable == null) return;
        _interactable.Interact(_player);
    }
}
