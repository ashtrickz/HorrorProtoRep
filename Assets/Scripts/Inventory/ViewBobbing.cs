using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Reflection.Editor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [Space]
    [SerializeField] private float intensity;
    [SerializeField] private float intensityX;
    [SerializeField] private float speed;

    private PositionFollower _follower;
    private Vector3 _originalOffset;
    private float _sinTime;

    private void Start()
    {
        _follower = GetComponent<PositionFollower>();
        _originalOffset = _follower.Offset;
    }

    private void Update()
    {
        Vector3 inputVector = new Vector3(inputManager.InputAxis.x, 0, inputManager.InputAxis.y);
        if (inputVector.magnitude > 0f)
        {
            _sinTime += Time.deltaTime * speed;
        }
        else
        {
            _sinTime = 0f;
        }
        
        float sinAmountY = -Mathf.Abs(intensity * Mathf.Sin(_sinTime));
        Vector3 sinAmountX = _follower.transform.right * intensity * Mathf.Cos(_sinTime) * intensityX;

        _follower.Offset = new Vector3(_originalOffset.x, _originalOffset.y + sinAmountY, _originalOffset.z);

        _follower.Offset += sinAmountX;
        _follower.Offset = new Vector3(_follower.Offset.x, _follower.Offset.y, _originalOffset.z);
    }
}