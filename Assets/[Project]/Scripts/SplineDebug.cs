using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineDebug : MonoBehaviour
{
    [SerializeField] private Spline _spline;
    [SerializeField] private Color _color;
    [SerializeField, Range(.01f, 10)] private float _raduis;
    [SerializeField, Range(0.1f, 10f)] private float _gap;

    void OnDrawGizmos()
    {
        if(!_spline)
            _spline = GetComponent<Spline>();
        if(!_spline || _gap < 0.1)
            return;

        Gizmos.color = _color;

        for(float l = 0; l < _spline.length(); l += _gap)
        {
            Gizmos.DrawSphere(transform.TransformPoint(_spline.computePointWithLength(l)), _raduis);
        }
    }
}
