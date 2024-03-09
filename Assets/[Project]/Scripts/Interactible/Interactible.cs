using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class Interactible : MonoBehaviour
{
    [SerializeField] public UnityEvent _onInteract;
    [SerializeField] private float _outLineAnimationDuration = .2f;
    [SerializeField] private AnimationCurve _outlinePopCurve;
    public  List<MeshRenderer> _rendererList;

    void Start()
    {
        InteractibleManager.AddInteractible(this);
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            if(!item.GetComponent<TextMeshPro>())
                _rendererList.Add(item);
        }

        for (int i = 0; i < _rendererList.Count; i++)
        {
            _rendererList[i].sharedMaterials[1] = new Material(_rendererList[i].sharedMaterials[1]);
            _rendererList[i].materials[1].SetFloat("_Scale", 0);
        }
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
            for (int i = 0; i < _rendererList.Count; i++)
                _rendererList[i].materials[1].SetFloat("_Scale", time);
        }, 0, 1.2f, _outLineAnimationDuration).SetEase(_outlinePopCurve);
    }

    public void OnUnselected()
    {
        DOTween.To((time) =>
        {
            for (int i = 0; i < _rendererList.Count; i++)
                _rendererList[i].materials[1].SetFloat("_Scale", time);
        }, 1.2f, 0, _outLineAnimationDuration).SetEase(Ease.Linear);
    }

    void OnDestroy()
    {
        InteractibleManager.RemoveInteractible(this);
    }
}
