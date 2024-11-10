using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventoryItem")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string itemStringID;
    [SerializeField, PreviewField(Alignment = ObjectFieldAlignment.Left)] private Sprite itemSprite;
    [SerializeField] private Vector3 inventoryPosition;
    [SerializeField] private AudioClip onUseSound;

    /*/ GETTERS /*/
    
    public string StringID => itemStringID;
    public Sprite Sprite => itemSprite;
    public Vector3 InventoryPosition => inventoryPosition;
    public AudioClip OnUseSound => onUseSound;

    /*/ METHODS /*/
    
    private void OnValidate()
    {
        if (itemStringID == string.Empty)
            itemStringID = name.Replace("Data", "");
    }
}
