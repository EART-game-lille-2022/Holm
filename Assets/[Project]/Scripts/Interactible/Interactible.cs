using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Color _selectedColor;

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
        print("S");
        // transform.localScale = Vector3.one * 1.2f;
        _renderer.sharedMaterials[1].SetFloat("_Scale", 1.2f);
    }

    public void OnUnselected()
    {
        print("U");
        // transform.localScale = Vector3.one;
        _renderer.sharedMaterials[1].SetFloat("_Scale", 0f);

    }
}
