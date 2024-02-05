using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindReciver : MonoBehaviour
{
    [SerializeField] private List<WindArea> _windAreasList;
    [SerializeField] private float _windForce;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _windForce = 0;
        foreach (var item in _windAreasList)
        {
            _windForce += item.Force;
        }

        _rigidbody.AddForce(Vector3.up * _windForce, ForceMode.Acceleration);
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
