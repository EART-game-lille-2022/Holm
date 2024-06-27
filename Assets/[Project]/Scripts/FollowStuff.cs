using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStuff : MonoBehaviour
{
    [SerializeField] private bool _followPlayer;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offSet;

    void Start()
    {
        if(_followPlayer)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            transform.rotation = _target.rotation;
        }
    }

    void OnValidate()
    {
        Update();
    }

    void Update()
    {
        if(_target)
            transform.position = _target.position + _offSet;
    }
}
