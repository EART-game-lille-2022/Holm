using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector3 force = Vector3.up * 100;
    [SerializeField, Range(0, 1)] private float directivity = .1f;
    private List<Rigidbody> bodies = new List<Rigidbody>();


    void FixedUpdate()
    {
        foreach (var b in bodies)
        {
            // b.AddForce(transform.rotation * force);
            b.velocity = Vector3.Slerp(b.velocity, force, directivity);
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
