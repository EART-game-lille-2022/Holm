using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private PlayerControler _playerControler;
    [SerializeField] private RectTransform _buttonRect;
    [SerializeField] private Vector2 _position;
    [SerializeField] private float _distance;
    [SerializeField] private Vector2 _direction;
    private bool _isMouseTracking = false;
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = (RectTransform)transform;
    }

    void Update()
    {
        _distance = Vector2.Distance(_position,  transform.position);
        _direction = _position - (Vector2)_rectTransform.position;

        if(_isMouseTracking)
        {
            _buttonRect.position = _position;

            if(_distance > _rectTransform.rect.width)
                _buttonRect.localPosition = _direction.normalized * _rectTransform.rect.width / 2;
        }

        if(!_isMouseTracking)
        {
            _direction = Vector2.zero;
            _buttonRect.localPosition = Vector3.zero;
        }

        _direction = _direction.normalized;
        _playerControler.SetMoveAxis(new Vector3(_direction.x, 0, _direction.y));
    }

    public void OnPointerUp(PointerEventData eventData)
    {        
        _isMouseTracking = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isMouseTracking = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _position = eventData.position;
    }
}
