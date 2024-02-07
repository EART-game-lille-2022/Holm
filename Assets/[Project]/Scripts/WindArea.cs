using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField] private Transform _direction;
    public Transform Direction { get => _direction; }
    [SerializeField] private float _force;
    public float Force { get => _force; }

    void OnTriggerEnter(Collider other)
    {
        WindReciver windReciver = other.GetComponentInParent<WindReciver>();
        if (windReciver)
        {
            windReciver.AddArea(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        WindReciver windReciver = other.GetComponentInParent<WindReciver>();
        if (windReciver)
        {
            windReciver.RemoveArea(this);
        }
    }
}
