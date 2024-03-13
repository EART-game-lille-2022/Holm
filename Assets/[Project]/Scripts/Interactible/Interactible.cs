using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Interactible : MonoBehaviour
{
    [SerializeField] public UnityEvent _onInteract;
    private MeshOutline _meshOutline;

    void Start()
    {
        _meshOutline = GetComponent<MeshOutline>();
        InteractibleManager.AddInteractible(this);
    }

    public void Interact()
    {
        print("Interact with : " + name);
        _onInteract.Invoke();
    }

    public void OnSelected()
    {
        _meshOutline.OnSelected();
    }

    public void OnUnselected()
    {
        _meshOutline.OnUnselected();
    }

    void OnDestroy()
    {
        InteractibleManager.RemoveInteractible(this);
    }
}
