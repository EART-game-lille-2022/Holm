using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiJoystick : MonoBehaviour
{
    [SerializeField] private RectTransform _buttonRect;
    [SerializeField] private Vector2 _position;
    [SerializeField] private float _distance;
    [SerializeField] private Vector2 _direction;
    private bool _isTracking = false;
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = (RectTransform)transform;
    }

    void Update()
    {
        _distance = Vector2.Distance(_position,  transform.position);
        _direction = _position - (Vector2)_rectTransform.position;

        if(_isTracking)
        {
            _buttonRect.position = _position;

            if(_distance > _rectTransform.rect.width)
                _buttonRect.localPosition = _direction.normalized * _rectTransform.rect.width / 2;
        }

        if(!_isTracking)
        {
            _direction = Vector2.zero;
            _buttonRect.localPosition = Vector3.zero;
        }

        _direction = _direction.normalized;
    }

    public Vector2 GetAxis()
    {
        return _direction;
    }

    public void OnPointerUp(PointerEventData eventData)
    {        
        _isTracking = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isTracking = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _position = eventData.position;
    }
}
