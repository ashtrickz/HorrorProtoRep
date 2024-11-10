using Sirenix.OdinInspector;
using UnityEngine;

namespace Interactables.Inventory.Items
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class InventoryItem : InteractableBase, IUsable
    {
        [Title("Inventory Item Properties", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private InventoryItemData itemData;
        [SerializeField] private bool initOnStart = true;

        /*/ PRIVATES /*/
        
        private    Collider _collider;
        private   Rigidbody _rigidbody;

        /*/ GETTERS/*/

        public InventoryItemData Data => itemData;
        public Collider Collider => _collider;
        public Rigidbody Rigidbody => _rigidbody;

        private void Awake()
        {
            InitComponents();
        }

        public override void Start()
        {
            base.Start();
            
            if (!initOnStart) return;
            InitComponents();
            Init(itemData);
        }

        private void InitComponents()
        {
            _collider       = GetComponent<Collider>();
            _rigidbody      = GetComponent<Rigidbody>();
        }

        public void Init(InventoryItemData data)
        {
            itemData = data;
            
            name = data.StringID;
            if (data.OnUseSound != null) AudioSource.clip = data.OnUseSound;
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            var inventory = player.InventoryManager;
            if (inventory.HasActiveItem) return;

            player.InventoryManager.PickUpItem(player, this);
        }

        public virtual void Use()
        {
            if (itemData.OnUseSound != null)
            {
                AudioSource.clip = itemData.OnUseSound;
                AudioSource.Play();
            }
        }
    }
}