using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string itemStringID;
    [SerializeField] private Sprite itemSprite;

    /*/ GETTERS /*/
    
    public string StringID => itemStringID;
    public Sprite Sprite => itemSprite;
    
}
