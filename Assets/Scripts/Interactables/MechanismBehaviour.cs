using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MechanismBehaviour : InteractableBase
{
    [Title("Mechanism Properties", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private InteractableBase[] triggerableInteractions;

    [Title("@_currentMechanismType", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] private EMechanismType mechanismType;
    [SerializeField, ShowIf("_isButton")] private Vector3 targetButtonPosition;
    [SerializeField, HideIf("_isButton")] private Vector3 targetSwitchRotation;

    private bool _isButton => mechanismType == EMechanismType.Button;
    private string _currentMechanismType => mechanismType.ToString() + " Properties";

    private Vector3 _originalPosition;
    private Vector3 _originalRotation;

    public override void Start()
    {
        base.Start();
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation.eulerAngles;
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        
        Interactable = false;
        switch (mechanismType)
        {
            case EMechanismType.Button:
                PressButton(player);
                break;
            case EMechanismType.Switch: 
                ToggleSwitch(player);
                break;
        }
        
    }

    private void PressButton(PlayerManager player)
    {
        transform.DOLocalMove(targetButtonPosition, MoveDuration).SetEase(AnimationEasing)
            .OnComplete(() =>
            {
                transform.DOLocalMove(_originalPosition, MoveDuration).SetEase(AnimationEasing)
                    .OnComplete(() => Interactable = true);
                TriggerInteractables(player);
            });
    }

    private void ToggleSwitch(PlayerManager player)
    {
        var currentTargetRotation = interactableState == EInteractableState.Released 
            ? _originalRotation
            : targetSwitchRotation ;
        transform.DOLocalRotate(currentTargetRotation, MoveDuration).SetEase(AnimationEasing)
            .OnComplete(() =>
            {
                TriggerInteractables(player);
                Interactable = true;
            });
    }

    private void TriggerInteractables(PlayerManager player)
    {
        foreach (var interactable in triggerableInteractions)
        {
            interactable.Interact(player);
        }
    }
    
    enum EMechanismType
    {
        Button,
        Switch,
    }
}