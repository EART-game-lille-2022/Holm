using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField] private float _force = 5;
    [SerializeField] private float _directionalForce = 0;
    [SerializeField] private List<Rigidbody> _rigidbodyList = new List<Rigidbody>();

    void Update()
    {
        if(_rigidbodyList.Count == 0)
            return;
        
        foreach (var item in _rigidbodyList)
        {
            item.AddForceAtPosition(transform.forward * _force
                                  , item.transform.TransformPoint(Vector3.up * _directionalForce)
                                  , ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb && !_rigidbodyList.Contains(rb))
            _rigidbodyList.Add(rb);
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb && _rigidbodyList.Contains(rb))
            _rigidbodyList.Remove(rb);
    }
}
