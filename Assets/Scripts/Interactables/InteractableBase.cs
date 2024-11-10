using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(AudioSource))]
public class InteractableBase : MonoBehaviour, IInteractable
{   
    [Title("Interactable Properties", TitleAlignment = TitleAlignments.Centered)]
    [SerializeField] protected AudioClip OnInteractSound;
    [SerializeField] protected EInteractableState interactableState;
    
    public bool Interactable = true;
    
    protected Outline OutlineManager;
    protected AudioSource AudioSource;

    public Outline Outline => OutlineManager;    
    
    public virtual void Start()
    {
        OutlineManager = GetComponent<Outline>();
        OutlineManager.ToggleOutline(false);

        AudioSource = GetComponent<AudioSource>();
    }
    
    public virtual void Interact(PlayerManager player)
    {
        if (!Interactable) return;

        if (OnInteractSound != null)
        {
            AudioSource.clip = OnInteractSound;
            AudioSource.Play();
        }
    }

    protected enum EInteractableState
    {
        Pressed,
        Released
    }
    
}
