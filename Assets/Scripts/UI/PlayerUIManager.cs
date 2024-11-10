using System;
using System.Collections;
using System.Collections.Generic;
using Interactables.Inventory.Items;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{

    #region Singleton

    private static PlayerUIManager _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static PlayerUIManager Instance => _instance;
    
    #endregion

    [SerializeField] private Transform inventorySlotContainer;

    [SerializeField] private Animator interactablePopupAnimator;
    
    private Dictionary<int, InventorySlotPresenter> _inventorySlotsDictionary = new();

    private bool _isIPopupActive = false;
    
    /*/ GETTERS /*/

    public RootData.RootGameActions GameActions => RootData.RootInstance.GameActions;

    /*/ METHODS /*/

    #region UnityLifeCycle

    public void Init()
    {
        var root = RootData.RootInstance;
        for (int i = 0; i < root.InventorySlotsCount; i++)
        {
            var slot = Instantiate(root.InventorySlotPresenterPrefab, inventorySlotContainer);
            slot.Clear();
            slot.ChangeSlotStatus(i == 0);
            _inventorySlotsDictionary.Add(i, slot);
        }
    }
    
    private void OnEnable()
    {
        GameActions.OnItemPickUpAction += AddItem;
        GameActions.OnItemDropAction   += RemoveItem;
        GameActions.OnItemSwitchAction += SelectSlot;
    }

    private void OnDisable()
    {
        GameActions.OnItemPickUpAction -= AddItem;
        GameActions.OnItemDropAction   -= RemoveItem;
        GameActions.OnItemSwitchAction -= SelectSlot;
    }

    #endregion

    #region ItemCallbacks

    private void AddItem(int slotID, InventoryItem item)
    {
        _inventorySlotsDictionary[slotID].Init(item.Data);
    }

    private void RemoveItem(int slotID, InventoryItem item = null)
    {
        _inventorySlotsDictionary[slotID].Clear();
    }

    private void SelectSlot(int slotID, InventoryItem item = null)
    {
        for (int i = 0; i < _inventorySlotsDictionary.Count; i++)
        {
            _inventorySlotsDictionary[i].ChangeSlotStatus(i == slotID);
        }
    }

    #endregion

    #region InteractablePopUp
    
    public void ToggleInteractablePopup(bool status)
    {
        if (_isIPopupActive == status)
            return;
        
        interactablePopupAnimator.Play(status ? "PopIn" : "PopOut");
        _isIPopupActive = status;
    }
    
    #endregion
}
