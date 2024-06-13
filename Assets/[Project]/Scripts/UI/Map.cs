using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform _mapPin;
    private Transform _player;

    void Start()
    {
        _player = PlayerInstance.instance.transform;
    }

    void Update()
    {
        if (!_mapPin)
            return;
        _mapPin.transform.position = new Vector3(_player.position.x, _mapPin.position.y, _player.position.z);
    }
}
