using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class InventoryItem : MonoBehaviour, IInteractable, IUsable
{
    [SerializeField] private Collider collider;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private Outline outlineManager;
    [SerializeField] private bool initOnStart = true;
    public bool Interactable = true;

    [Space, SerializeField] private GameObject fireObject; 
    
    /*/ GETTERS/*/

    public InventoryItemData Data => itemData;
    public Collider Collider => collider;
    public Rigidbody Rigidbody => rigidbody;

    private void Start()
    {
        if (initOnStart) Init(itemData);
    }
    
    public void Init(InventoryItemData data)
    {
        itemData = data;
        outlineManager.ToggleOutline(false);
    }
    
    public void Interact(PlayerManager player)
    {
        if (!Interactable) return;
        
        var inventory = player.InventoryManager;
        if (inventory.HasActiveItem) return;

        player.InventoryManager.PickUpItem(player, this);
    }

    public void Use()
    {
        ToggleParticles();
    }

    private void ToggleParticles()
    {
        fireObject.SetActive(!fireObject.activeSelf);
    }
}