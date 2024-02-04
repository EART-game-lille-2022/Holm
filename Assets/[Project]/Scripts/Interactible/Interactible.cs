using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float _outLineAnimationDuration;
    [SerializeField] private AnimationCurve _outlinePopCurve;

    void Start()
    {
        InteractibleManager.AddInteractible(this);
        _renderer.sharedMaterials[1] = new Material(_renderer.sharedMaterials[1]);
        _renderer.materials[1].SetFloat("_Scale", 0);
    }

    public void Interact()
    {
        print("Interact with : " + name);
        _onInteract.Invoke();
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
        //TODO Animation bug !
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
