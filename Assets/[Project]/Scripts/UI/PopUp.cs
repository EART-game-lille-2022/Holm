using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private float _offSetTarget = 200;
    [SerializeField] private float _duration = 5;
    [SerializeField] private AnimationCurve _curve;
    private Vector2 _startPos;
    private Color _startColor;
    private RectTransform _rectTransform;
    private TextMeshProUGUI _text;
    private float _time = 0;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _startColor = _text.color;
        _rectTransform = (RectTransform)transform;
        _startPos = _rectTransform.anchoredPosition;
    }

    void Update()
    {
        _time += Time.deltaTime / _duration;

        _rectTransform.anchoredPosition =
        Vector2.Lerp(_startPos, new Vector2(_rectTransform.anchoredPosition.x, _offSetTarget), _time);
        _text.color = 
        Color.Lerp(_startColor, new Color(0, 0, 0, 0), _curve.Evaluate(_time));

        if(_time > 1)
            Destroy(gameObject);
    }
}
