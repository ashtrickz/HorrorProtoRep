using Sirenix.OdinInspector;
using UnityEngine;

namespace Interactables.Inventory.Items
{
    public class ToggleableInventoryItem : InventoryItem
    {
        [Title("Toggleable II Properties", titleAlignment: TitleAlignments.Centered)]
        [Space, SerializeField] private GameObject toggleObject;

        public override void Use()
        {
            base.Use();
            Toggle();
        }

        private void Toggle() =>
            toggleObject.SetActive(!toggleObject.activeSelf);
    }
}