using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindReciver : MonoBehaviour
{
    [SerializeField] private List<WindArea> _windAreasList;
    [SerializeField] private float _windForce;
    [SerializeField] private Vector3 _windDirection;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(_windAreasList.Count == 0)
            return;

        Vector3 mediumDirection = Vector3.zero;

        _windForce = 0;
        _windDirection = Vector3.zero;
        foreach (var item in _windAreasList)
        {
            _windForce += item.Force;
            _windDirection += item.Direction.up;
            mediumDirection += item.transform.position;
        }

        Vector3 forcePos = (mediumDirection / _windAreasList.Count).normalized;

        print(forcePos);
        // _rigidbody.AddForce(_windDirection.normalized * _windForce, ForceMode.Acceleration);
        _rigidbody.AddForceAtPosition(_windDirection * _windForce, transform.TransformPoint(forcePos), ForceMode.Force);
    }

    public void AddArea(WindArea area)
    {
        if (!_windAreasList.Contains(area))
            _windAreasList.Add(area);
    }

    public void RemoveArea(WindArea area)
    {
        if (_windAreasList.Contains(area))
            _windAreasList.Remove(area);
    }
}
