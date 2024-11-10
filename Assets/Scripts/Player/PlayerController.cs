using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [Space, Title("Speed Parameters", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float staminaAmount;
    
    [Space, Title("Gravity Parameters", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float gravityMultiplier = 3f;
    
    [Space, Title("State Change Parameters", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private float headStateChangeDuration = .5f;
    [SerializeField] private float defaultHeadHeight = 0.453f, onCrouchHeadHeight = 0.072f;
    [SerializeField] private float playerDefaultHeight = 2, playerCrouchHeight = 1;

    private CharacterController _controller;
    private ViewBobbing _viewBobbing;
    
    private Vector2 _rotation = Vector2.zero;
    private Vector3 _moveDirection;

    private float _velocity;
    private bool _isCrouching = false;
    
    private PlayerManager _player;
    private InputManager InputManager => _player.InputManager;
    private CameraManager CameraManager => _player.CameraManager;

    public void Init(PlayerManager playerManager)
    {
        _player = playerManager;
        _controller = GetComponent<CharacterController>();
        _viewBobbing = GetComponentInChildren<ViewBobbing>();

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
        
        if (_isCrouching) _moveDirection *= crouchSpeed;
        else _moveDirection *= (InputManager.IsSprinting ? runSpeed : walkSpeed);

        _controller.Move(_moveDirection * Time.fixedDeltaTime);
    }

    private void HandleGravity()
    {
        if (_controller.isGrounded && _velocity < 0f) _velocity = -1f;
        else _velocity -= gravityForce * gravityMultiplier * Time.fixedDeltaTime;

        _moveDirection.y = _velocity;
    }

    private void HandleJump()
    {
        if (InputManager.IsJumping && _controller.isGrounded) _velocity += jumpForce;
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

    public void ToggleCrouch()
    {
        _isCrouching = !_isCrouching;
        if (_isCrouching) _viewBobbing.enabled = false;

        _controller.center = new Vector3(0, _isCrouching ? -.5f : 0);
        _controller.height = _isCrouching ? playerCrouchHeight : playerDefaultHeight;

        _viewBobbing.transform.DOLocalMoveY(_isCrouching ? onCrouchHeadHeight : defaultHeadHeight, headStateChangeDuration)
            .OnComplete(() =>
            {
                if (!_isCrouching) _viewBobbing.enabled = true;
            });
    }
    
}