using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Space, Title("Managers", TitleAlignment = TitleAlignments.Centered)] 
    [SerializeField] private   PlayerController playerController;
    
    [SerializeField, InlineEditor()] private       InputManager inputManager;
    [SerializeField, InlineEditor()] private      CameraManager cameraManager;
    [SerializeField, InlineEditor()] private InteractionManager interactionManager;
    [SerializeField, InlineEditor()] private   InventoryManager inventoryManager;

    public Camera PlayerCamera => Camera.main;
    public InputManager InputManager => inputManager;
    public CameraManager CameraManager => cameraManager;
    public InventoryManager InventoryManager => inventoryManager;

    private void Awake()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        PlayerUIManager.Instance.Init();
        inputManager.Init(this);
        cameraManager.Init(this);
        playerController.Init(this);
        inventoryManager.Init(this);
        interactionManager.Init(this);
    }
}
