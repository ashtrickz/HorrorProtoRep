﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        private InputMap _inputMap;
    
        /*/ GETTERS /*/
    
        public bool IsActive     => _inputMap.asset.enabled;
        public bool IsMoving     => _inputMap.InGameIM.Move.IsPressed();
        public bool IsSprinting  => _inputMap.InGameIM.Sprint.IsPressed();
        public bool IsJumping    => _inputMap.InGameIM.Jump.IsPressed();
        public Vector2 InputAxis => _inputMap.InGameIM.Move.ReadValue<Vector2>();
    
        [DisplayAsString] public Vector2 CameraInput;

        private PlayerManager _player;

        public void Init(PlayerManager playerManager)
        {
            _player = playerManager;
        
            _inputMap = new InputMap();
            _inputMap.Enable();
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            CameraInput = ctx.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            if (_inputMap != null)
                _inputMap.Enable();
        }

        private void OnDisable()
        { 
            if (_inputMap != null) 
                _inputMap.Disable();
        }
    }
}