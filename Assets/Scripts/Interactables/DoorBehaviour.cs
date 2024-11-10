using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class DoorBehaviour : InteractableBase
{
    [Title("Door Properties", TitleAlignment =  TitleAlignments.Centered)]
    [SerializeField] private EOpenDirection openDirection;

    private Vector3 _originalRotation;
    private Vector3 _openedRotation;
    private Vector3 _closedRotation;

    private void OnValidate()
    {
        CalculateRotations();
    }

    public override void Start()
    {
        base.Start();

        _originalRotation = transform.rotation.eulerAngles;
        CalculateRotations();
    }

    private void CalculateRotations()
    {
        var targetRotation = new Vector3(
            _originalRotation.x,
            _originalRotation.y + (openDirection == EOpenDirection.Forwards ? -90 : 90),
            _openedRotation.z);

        _openedRotation = interactableState == EInteractableState.Pressed  //Opened in context of a Door
            ? _originalRotation
            : targetRotation;  
        _closedRotation = interactableState == EInteractableState.Released  //Closed in context of a Door
            ? _originalRotation 
            : targetRotation; 
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        switch (interactableState)
        {
            //Opened in context of a Door
            case EInteractableState.Released:
                transform.DORotate(_closedRotation, MoveDuration).SetEase(AnimationEasing);
                interactableState = EInteractableState.Released;
                break;
            //Closed in context of a Door
            case EInteractableState.Pressed:
                transform.DORotate(_openedRotation, MoveDuration).SetEase(AnimationEasing);
                interactableState = EInteractableState.Pressed;
                break;
        }
    }
    
    enum EOpenDirection
    {
        Forwards,
        Backwards
    }
}
