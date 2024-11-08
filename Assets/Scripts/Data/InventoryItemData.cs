using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventoryItem")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string itemStringID;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Vector3 inventoryPosition;

    /*/ GETTERS /*/
    
    public string StringID => itemStringID;
    public Sprite Sprite => itemSprite;
    public Vector3 InventoryPosition => inventoryPosition;

}
