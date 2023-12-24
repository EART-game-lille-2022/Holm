using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private RectTransform _backGroundRect;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 _direction;
    private bool _isMouseTracking = false;


    void Update()
    {
        distance = Vector2.Distance(_backGroundRect.position,  transform.position);
        if(_isMouseTracking)
        {
            _direction = _position - _backGroundRect.position;
            _direction = Vector3.Normalize(_direction);

            transform.position = _position;
        }
        else
        {
            transform.position = _backGroundRect.position;
        }
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
