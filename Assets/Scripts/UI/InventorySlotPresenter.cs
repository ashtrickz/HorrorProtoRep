using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotPresenter : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private Image itemImage;

    private InventoryItemData _currentItemData;

    public void Init(InventoryItemData itemData) // On Pick Up
    {
        if (itemData != null)
        {
            _currentItemData = itemData;
            itemImage.sprite = itemData.Sprite;
        }
        else
        {
            
        }
        itemImage.color = new Color32(255, 255, 255, 255);
    }

    public void Clear() //On Drop
    {
        _currentItemData = null;
        itemImage.sprite = null;
        itemImage.color = new Color32(255, 255, 255, 0);
    }

    public void ChangeSlotStatus(bool selected) // On Item Slot Change
    {
        if (selected) SelectSlot();
        else DeselectSlot();
    }

    private void SelectSlot()
    {
        borderImage.color = Color.green;
    }

    private void DeselectSlot()
    {
        borderImage.color = Color.gray;
    }
}