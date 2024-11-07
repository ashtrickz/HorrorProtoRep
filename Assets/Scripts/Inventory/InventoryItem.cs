using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class InventoryItem : MonoBehaviour, IInteractable
{
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private InventoryItemData itemData;
    
    public bool Interactable = true;
    
    /*/ GETTERS/*/

    public InventoryItemData Data => itemData;
    public Collider Collider => collider;
    public Rigidbody Rigidbody => rigidbody;

    public void Init(InventoryItemData data)
    {
        itemData = data;
    }
    
    public void Interact(PlayerManager player)
    {
        if (!Interactable) return;
        
        var inventory = player.InventoryManager;
        if (inventory.HasActiveItem) return;

        player.InventoryManager.PickUpItem(player, this);
    }
}
