using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStuff : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offSet;

    void OnValidate()
    {
        Update();
    }

    void Update()
    {
        transform.position = _target.position + _offSet;
    }
}
