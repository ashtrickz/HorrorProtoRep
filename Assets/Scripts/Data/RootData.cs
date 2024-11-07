using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RootData")]
public class RootData : SerializedScriptableObject
{

    #region Singleton

    private static RootData _instance;
    
    public static RootData RootInstance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Loading... RootData instance");
#endif
                _instance = Resources.Load<RootData>("Data/RootData");
                if (_instance == null)
                {
                    Debug.LogError("RootData doesn't found");
#if UNITY_EDITOR
                    Debug.Log($"Creating... RootData instance");
#endif
                    _instance = CreateInstance<RootData>();
                }
            }

            return _instance;
        }
    }

    #endregion

    [Header("General Data")] 
    [SerializeField] private int inventorySlotsCount = 2;

    /*/ PREFABS /*/

    [Header("Prefabs")] 
    [SerializeField] private InventorySlotPresenter inventorySlotPresenterPrefab;

    [field: SerializeField] private Dictionary<string, InventoryItem> inventoryItems = new();

    /*/ GETTERS /*/

    public int InventorySlotsCount => inventorySlotsCount;
    public RootGameActions GameActions = new();
    public InventorySlotPresenter InventorySlotPresenterPrefab => inventorySlotPresenterPrefab;

    /*/ EVENTS /*/
    
    public class RootGameActions
    {
        public Action<int, InventoryItem> OnItemPickUpAction;
        public Action<int, InventoryItem> OnItemDropAction;
        public Action<int, InventoryItem> OnItemSwitchAction;
    }
}






