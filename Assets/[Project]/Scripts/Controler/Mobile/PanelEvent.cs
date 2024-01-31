using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanelEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private UnityEvent<PointerEventData> _onPointerDown;
    [SerializeField] private UnityEvent<PointerEventData> _onPointerMove;
    [SerializeField] private UnityEvent<PointerEventData> _onPointerUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown.Invoke(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _onPointerMove.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp.Invoke(eventData);
    }
}
