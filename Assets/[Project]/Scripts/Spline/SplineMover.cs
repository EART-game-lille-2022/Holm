using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplineMover : MonoBehaviour
{
    [SerializeField] private bool _isLooping;
    [SerializeField] private Spline _spline;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceOffSet;
    public bool _isMoving = true;
    [SerializeField] private bool _followRotation;
    private float _distance;
    private Vector3 _newPosition = Vector3.zero;
    private Vector3 _lastFramePosition;
    private Vector3 _startPosition;

    void Start()
    {
        _distance += _distanceOffSet;
        _startPosition = transform.position;
    }

    void Update()
    {
        if (!_spline)
        {
            print("Spline not Set to container !");
            return;
        }

        if (!_isMoving)
            return;

        _distance += Time.deltaTime * _speed;
        _newPosition = _spline.computePointWithLength(_distance);

        transform.position = _spline.transform.TransformPoint(_newPosition);

        if(_followRotation)
            transform.forward = _spline.computeOrientationWithLength(_distance, Vector3.up).forward;


        if (transform.position == _lastFramePosition)
        {
            if (_isLooping)
            {
                _distance = 0;
                return;
            }
            _isMoving = false;
        }

        _lastFramePosition = transform.position;
    }

    public void SetSpeed(float value)
    {
        _speed = value;
    }

    public void LerpSpeed(float value)
    {
        float oldSpeed = _speed;
        DOTween.To((time) =>
        {
            _speed = time;
        }, oldSpeed, value, .5f);
    }

    public void CanMove(bool value)
    {
        _isMoving = value;
    }
}
