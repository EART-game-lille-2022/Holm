using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 _axisToRotate;
    [SerializeField] private float _speed;

    private float randomeSpeed;

    void Start()
    {
        randomeSpeed = Random.Range(.5f, 1.5f);
    }

    void Update()
    {
        transform.Rotate(_axisToRotate * _speed * randomeSpeed);
    }
}
