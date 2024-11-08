using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    
    private PlayerManager _player;
    private InputMap _inputMap;

    private (int, InventoryItem) _activeItem;
    private Dictionary<int, InventoryItem> _inventoryDictionary = new() {};

    private Transform _worldItemsContainer;

    /*/ GETTERS /*/

    public RootData.RootGameActions GameActions => RootData.RootInstance.GameActions;
    public bool HasActiveItem => _activeItem.Item2;

    /*/ METHODS /*/
    
    public void Init(PlayerManager playerManager)
    {
        _player = playerManager;
        _inventoryDictionary.Clear();

        var root = RootData.RootInstance;

        for (int i = 0; i < root.InventorySlotsCount; i++)
            _inventoryDictionary.Add(i, null);

        _worldItemsContainer = GameObject.FindWithTag("WorldItemsContainer").transform;
    }
    
    #region UnityLifeCycle

    private void OnEnable()
    {
        GameActions.OnItemPickUpAction += InventoryAddItem;
        GameActions.OnItemDropAction   += InventoryRemoveItem;
        GameActions.OnItemSwitchAction += InventorySwitchItem;
    }

    private void OnDisable()
    {
        GameActions.OnItemPickUpAction -= InventoryAddItem;
        GameActions.OnItemDropAction   -= InventoryRemoveItem;
        GameActions.OnItemSwitchAction -= InventorySwitchItem;
    }

    #endregion

    #region PickUpItem
    
    public void PickUpItem(PlayerManager player, InventoryItem item)
    {
        item.Interactable = false;
        item.Collider.enabled = false;
        item.Rigidbody.useGravity = false;
        item.Rigidbody.isKinematic = true;
        item.transform.SetParent(itemContainer);
        item.transform.position = itemContainer.position;

        item.transform.localPosition = item.Data.InventoryPosition;
        item.transform.localRotation = Quaternion.Euler(0, 0, 0);

#if UNITY_EDITOR
        Debug.Log($"Added {item.gameObject.name} for {player.gameObject.name}");
#endif        
        
        if (_player == player) 
            GameActions.OnItemPickUpAction.Invoke(GetSlotID(), item);
    }

    private void InventoryAddItem(int slotID, InventoryItem item = null)
    {
        _inventoryDictionary[slotID] = item;
        _activeItem = (slotID, item);
    }
    
    #endregion

    #region SwitchItem

    public void SwitchItem(int slotId)
    {
        if (slotId >= RootData.RootInstance.InventorySlotsCount) return;
        
        var oldItem = _inventoryDictionary[_activeItem.Item1];
        var newItem = _inventoryDictionary[slotId];
        
        if (oldItem != null)
            oldItem.gameObject.SetActive(false);

        if (newItem != null)
            newItem.gameObject.SetActive(true);
        
        GameActions.OnItemSwitchAction.Invoke(slotId, newItem);
    }

    private void InventorySwitchItem(int slotId, InventoryItem item)
    {
        _activeItem = (slotId, item);
    }
    
    #endregion
    
    #region DropItem

    public void DropItem()
    {
        var item = _activeItem.Item2;
     
        if (item == null) return;
        
        item.Interactable = true;
        item.Collider.enabled = true;
        item.Rigidbody.useGravity = true;
        item.Rigidbody.isKinematic = false;
        item.transform.SetParent(_worldItemsContainer);
        //item.transform.position = itemContainer.position;
        
        item.Rigidbody.AddForce((_player.transform.forward * 2) + (_player.transform.up * 2), ForceMode.Force);

#if UNITY_EDITOR
        Debug.Log($"Removed {item.gameObject.name} from {_player.gameObject.name}");
#endif
        
        GameActions.OnItemDropAction.Invoke(GetSlotID(item), item);
    }
    
    private void InventoryRemoveItem(int slotID, InventoryItem item = null)
    {
        _inventoryDictionary[slotID] = null;
        _activeItem = (slotID, null);
    }
    
    #endregion
    
    private int GetSlotID(InventoryItem item = null)
    {
        var slotID = -1;
        foreach (var slot in _inventoryDictionary)
        {
            if (slot.Value != item) continue;
            slotID = slot.Key;
            break;
        }

        if (slotID == -1)
            throw new Exception(
                $"{_player.gameObject.name} has no slot with {item}!");

        return slotID;
    }
    
}
