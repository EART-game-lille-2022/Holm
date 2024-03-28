using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshControler : MonoBehaviour
{
    [SerializeField] private Transform _controlerTransform;
    [SerializeField] private Vector3 _positionOffset;

    void Update()
    {
        transform.position = _controlerTransform.position + _positionOffset;
        transform.rotation = _controlerTransform.rotation;
    }

    void OnValidate()
    {
        transform.position = _controlerTransform.position + _positionOffset;
        transform.rotation = _controlerTransform.rotation;
    }
}
