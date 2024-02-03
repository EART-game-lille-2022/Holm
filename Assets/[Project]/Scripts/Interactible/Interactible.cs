using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent _onInteract;

    void Start()
    {
        InteractibleManager.AddInteractible(this);
    }

    public void Interact()
    {
        print("Interact with : " + name);
        _onInteract.Invoke();
    }

    public void OnSelected()
    {
        transform.localScale = Vector3.one * 1.2f;
    }

    public void OnUnselected()
    {
        transform.localScale = Vector3.one;
    }
}
