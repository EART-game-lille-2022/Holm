using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    // public Vector3 _force = Vector3.up * 100;
    [SerializeField] private float _force = 50;
    [SerializeField, Range(0, 1)] private float directivity = .1f;
    private List<Rigidbody> bodies = new List<Rigidbody>();


    void FixedUpdate()
    {
        foreach (var item in bodies)
        {
            // b.AddForce(transform.rotation * force);
            item.velocity = Vector3.Slerp(item.velocity, transform.up * _force, directivity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody body = other.gameObject.GetComponentInParent<Rigidbody>();
        if (body && !bodies.Contains(body))
        {
            bodies.Add(body);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody body = other.gameObject.GetComponentInParent<Rigidbody>();
        if (body && bodies.Contains(body))
        {
            bodies.Remove(body);
        }
    }
}
