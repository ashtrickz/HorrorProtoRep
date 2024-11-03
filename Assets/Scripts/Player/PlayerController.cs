using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [Space, Title("Managers", TitleAlignment = TitleAlignments.Centered)] [SerializeField]
    private InputManager inputManager;

    [SerializeField] private CameraManager cameraManager;

    [Space, Title("Controller Settings", TitleAlignment = TitleAlignments.Centered)] [SerializeField]
    private float walkSpeed;

    [SerializeField] private float runSpeed;
    [SerializeField] private float staminaAmount;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float gravityMultiplier = 3f;

    private Vector3 _moveDirection;
    private float _velocity;

    public void FixedUpdate()
    {
        HandleGravity();
        HandleJump();
        HandleMovement();
    }

    public void Update()
    {
        HandleRotation();
        HandleCamera  ();
    }

    private void HandleCamera()
    {
        if (inputManager.InputAxis == Vector2.zero) cameraManager.ManageAnimationSpeed(0.25f);
        else cameraManager.ManageAnimationSpeed(inputManager.IsSprinting ? 2 : 1);
    }

    private void HandleMovement()
    {
        if (!inputManager.IsActive) return;

        _moveDirection = new Vector3(inputManager.InputAxis.x, _moveDirection.y, inputManager.InputAxis.y);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= (inputManager.IsSprinting ? runSpeed : walkSpeed);

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
        if (inputManager.IsJumping && controller.isGrounded) _velocity += jumpForce;
    }

    private void HandleRotation()
    {
        var im = inputManager;
        var cm = cameraManager;

        transform.Rotate(Vector3.up, im.CameraInput.x * cm.Sensitivity.x * Time.deltaTime);
        var pitch = -im.CameraInput.y * cm.Sensitivity.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -cm.CameraYRange, cm.CameraYRange);
        cm.CmCamera.transform.localEulerAngles = new Vector3(pitch, cm.CmCamera.transform.localEulerAngles.y, 0f);
    }
}