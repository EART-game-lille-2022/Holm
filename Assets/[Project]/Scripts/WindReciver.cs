using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindReciver : MonoBehaviour
{
    [SerializeField] private List<WindArea> _windAreasList;
    public float _maxChaos;
    public float _minChaos;
    private Rigidbody _rigidbody;
    private Vector3 _offSet;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_windAreasList.Count == 0)
            return;

        if (_windAreasList[0].IsChaos)
            _offSet.x = Mathf.Lerp(_minChaos, _maxChaos, Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time)));
        else
            _offSet.x = 0;

        _rigidbody.AddForceAtPosition(_windAreasList[0].transform.up * _windAreasList[0].Force
                                     , transform.TransformPoint(Vector3.up + _offSet), ForceMode.Acceleration);
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
