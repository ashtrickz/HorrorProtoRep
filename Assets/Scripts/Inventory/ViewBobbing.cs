using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [Space] [SerializeField] private float intensity;
    [SerializeField] private float intensityX;
    [SerializeField] private float speed;

    [Space, DisplayAsString]
    [SerializeField] private Vector3 _bobbingOffset;

    private Vector3 _originalPosition;
    private Vector3 _originalOffset;
    private float _sinTime;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
        _originalOffset = _bobbingOffset;
    }

    private void Update()
    {
        CalculateSinTime();
        CalculateOffset();
        UpdatePosition();
    }

    private void CalculateSinTime()
    {
        var currentSpeed = inputManager.IsSprinting ? speed * 2 : speed;
        Vector3 inputVector = new Vector3(inputManager.InputAxis.x, 0, inputManager.InputAxis.y);
        if (inputVector.magnitude > 0f)
            _sinTime += Time.deltaTime * currentSpeed;
        else if (_sinTime > 1)
            _sinTime = 1;
        else
            _sinTime = Mathf.Lerp(_sinTime, 0, Time.deltaTime * speed);
    }

    private void CalculateOffset()
    {
        var currentIntensity = inputManager.IsSprinting ? intensity * 2 : intensity;
        float sinAmountY = -Mathf.Abs(currentIntensity * Mathf.Sin(_sinTime));
        Vector3 sinAmountX = transform.right * currentIntensity * Mathf.Cos(_sinTime) * intensityX;

        _bobbingOffset = new Vector3(_originalOffset.x, _originalOffset.y + sinAmountY, _originalOffset.z);
        _bobbingOffset += sinAmountX;
    }

    private void UpdatePosition() =>
        transform.localPosition = _originalPosition + _bobbingOffset;
}