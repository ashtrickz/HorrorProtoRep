using System;
using Cinemachine;
using Sirenix.Utilities.Editor.Expressions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")] 
    [SerializeField]
    private Vector2 sensitivity;
    [SerializeField, Range(0, 100)]
    private float cameraYRange;

    [SerializeField] private CinemachineVirtualCamera cmCamera;
    [SerializeField] private CinemachineCameraOffset cameraOffset;
    [SerializeField] private Animator cameraAnimator;
    
    private bool _shakeCamera = false;

    public CinemachineVirtualCamera CmCamera => cmCamera;
    
    public Vector2 Sensitivity => sensitivity;
    public float CameraYRange => cameraYRange;

    private void Update()
    {
        if (_shakeCamera) return;
        
        if (cameraOffset.m_Offset == Vector3.zero) return;
            
        cameraOffset.m_Offset = Vector3.SlerpUnclamped(cameraOffset.m_Offset, Vector3.zero, Time.deltaTime * 10f);
    }

    public void ToggleShake(bool state)
    {
        if (_shakeCamera != state)
            cameraAnimator.SetTrigger(state ? "StartShake" : "StopShake");
        _shakeCamera = state;
    }

    public void ManageAnimationSpeed(float speed)
    {
        if (cameraAnimator.speed == speed) return;
        cameraAnimator.speed = speed;
    }
}