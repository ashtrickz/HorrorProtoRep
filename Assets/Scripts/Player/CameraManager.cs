using System;
using Cinemachine;
using Sirenix.Utilities.Editor.Expressions;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")] 
    [SerializeField]                private Vector2 sensitivity;
    [SerializeField]                private   float verticalSensMultiplier;
    [SerializeField, Range(0, 100)] private   float cameraYRange;
    [SerializeField, Range(0, 1)]   private   float accelDecelTime = 0;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera cmCamera;
    [SerializeField] private                 Animator cameraAnimator;

    private CinemachinePOV _pov;

    public Vector2 Sensitivity => sensitivity;

    private PlayerManager _player;
    
    public void Init(PlayerManager playerManager)
    {
        _player = playerManager;
        
        cameraAnimator.SetTrigger("StartShake");
        
        _pov = GetComponentInChildren<CinemachinePOV>();
        _pov.m_VerticalAxis.m_MaxSpeed = sensitivity.y * verticalSensMultiplier;
        _pov.m_VerticalAxis.m_MinValue = -cameraYRange;
        _pov.m_VerticalAxis.m_MaxValue = cameraYRange;
        _pov.m_VerticalAxis.m_AccelTime = _pov.m_VerticalAxis.m_DecelTime = accelDecelTime;
    }

    public void ManageAnimationSpeed(float speed)
    {
        if (cameraAnimator.speed == speed) return;
        cameraAnimator.speed = speed;
    }
}