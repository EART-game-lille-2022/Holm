using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent<Interactible> _onInteract;
    [SerializeField] private float _outLineAnimationDuration = .2f;
    [SerializeField] private AnimationCurve _outlinePopCurve;
    private MeshRenderer _renderer;

    void Start()
    {
        InteractibleManager.AddInteractible(this);
        _renderer = GetComponentInChildren<MeshRenderer>();
        _renderer.sharedMaterials[1] = new Material(_renderer.sharedMaterials[1]);
        _renderer.materials[1].SetFloat("_Scale", 0);
    }

    public void Interact()
    {
        print("Interact with : " + name);
        _onInteract.Invoke(this);
    }

    public void EndInteraction()
    {
        InteractibleManager.instance.OnEndInteraction();
    }

    public void OnSelected()
    {
        DOTween.To((time) =>
        {
            _renderer.materials[1].SetFloat("_Scale", time);
        }, 0, 1.2f, _outLineAnimationDuration).SetEase(_outlinePopCurve);
    }

    public void OnUnselected()
    {
        DOTween.To((time) =>
        {
            _renderer.materials[1].SetFloat("_Scale", time);
        }, 1.2f, 0, _outLineAnimationDuration).SetEase(Ease.Linear);
    }

    void OnDestroy()
    {
        InteractibleManager.RemoveInteractible(this);
    }
}
