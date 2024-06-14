using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance;
    [SerializeField] private Transform _playerMapPin;
    [SerializeField] private GameObject _packagePin;
    private Transform _player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _player = PlayerInstance.instance.transform;
    }

    void Update()
    {
        if (!_playerMapPin)
            return;

        _playerMapPin.transform.position = new Vector3(_player.position.x, _playerMapPin.position.y, _player.position.z);
    }

    public void AddPackagePin(Vector3 pos)
    {
        GameObject newPin = Instantiate(_packagePin, transform);
        newPin.transform.position = new Vector3(pos.x, _playerMapPin.position.y, pos.z);
    }
}
