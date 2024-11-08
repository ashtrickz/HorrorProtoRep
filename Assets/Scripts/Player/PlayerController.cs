using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [Space, Title("Controller Settings", TitleAlignment = TitleAlignments.Centered)] [SerializeField]
    private float walkSpeed;

    [SerializeField] private float runSpeed;
    [SerializeField] private float staminaAmount;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float gravityMultiplier = 3f;

    private Vector2 _rotation = Vector2.zero;
    private Vector3 _moveDirection;
    private float _velocity;

    private PlayerManager _player;
    private InputManager InputManager => _player.InputManager;
    private CameraManager CameraManager => _player.CameraManager;

    public void Init(PlayerManager playerManager)
    {
        _player = playerManager;
    }
    
    public void FixedUpdate()
    {
        HandleGravity();
        HandleJump();
        HandleMovement();
    }

    public void Update()
    {
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (!InputManager.IsActive) return;

        _moveDirection = new Vector3(InputManager.InputAxis.x, _moveDirection.y, InputManager.InputAxis.y);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= (InputManager.IsSprinting ? runSpeed : walkSpeed);

        controller.Move(_moveDirection * Time.fixedDeltaTime);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && _velocity < 0f) _velocity = -1f;
        else _velocity -= gravityForce * gravityMultiplier * Time.fixedDeltaTime;

        _moveDirection.y = _velocity;
    }

    private void HandleJump()
    {
        if (InputManager.IsJumping && controller.isGrounded) _velocity += jumpForce;
    }

    private void HandleRotation()
    {
        var cm = CameraManager;
        var im = InputManager;
        
        _rotation.x += im.CameraInput.x * (cm.Sensitivity.x * 0.25f);
        _rotation.y += im.CameraInput.y * (cm.Sensitivity.y * 0.25f);
        _rotation.y = Mathf.Clamp(_rotation.y, -cm.CameraYRange, cm.CameraYRange);
        var xQuaternion = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuaternion = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        transform.localRotation = xQuaternion;
        CameraManager.CinemachineCam.transform.localRotation = yQuaternion;
    }
}