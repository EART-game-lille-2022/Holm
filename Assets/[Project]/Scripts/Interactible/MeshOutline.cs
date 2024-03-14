using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MeshOutline : MonoBehaviour
{
    [SerializeField] private float _animationDuration = .2f;
    private List<MeshRenderer> _rendererList = new List<MeshRenderer>();

    void Start()
    {
        //! Get all Mesh (exculde TM mesh)
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            if (!item.GetComponent<TextMeshPro>())
                _rendererList.Add(item);
        }

        //! Shader stuff
        for (int i = 0; i < _rendererList.Count; i++)
        {
            _rendererList[i].sharedMaterials[1] = new Material(_rendererList[i].sharedMaterials[1]);
            _rendererList[i].materials[1].SetFloat("_Scale", 0);
        }
    }

    public void OnSelected()
    {
        DOTween.To((time) =>
        {
            for (int i = 0; i < _rendererList.Count; i++)
                _rendererList[i].materials[1].SetFloat("_Scale", time);
        }, 0, 1.2f, _animationDuration).SetEase(Ease.Linear);
    }

    public void OnUnselected()
    {
        DOTween.To((time) =>
        {
            for (int i = 0; i < _rendererList.Count; i++)
                _rendererList[i].materials[1].SetFloat("_Scale", time);
        }, 1.2f, 0, _animationDuration).SetEase(Ease.Linear);
    }

    public void HideOutline()
    {
        for (int i = 0; i < _rendererList.Count; i++)
            _rendererList[i].materials[1].SetFloat("_Scale", 0);
    }
}
