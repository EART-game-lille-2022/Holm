using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeoutScreen : MonoBehaviour
{
    public static FadeoutScreen instance;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _cloudLeft;
    [SerializeField] private RectTransform _cloudRight;
    [SerializeField] private RectTransform _cloudBot;

    void Awake()
    {
        instance = this;
        _cloudLeft.gameObject.SetActive(false);
        _cloudRight.gameObject.SetActive(false);
    }

    public void CloudFade(bool startOpen, float duration, Action todoAfter = null)
    {
        _cloudLeft.gameObject.SetActive(true);
        _cloudRight.gameObject.SetActive(true);
        _cloudBot.gameObject.SetActive(true);

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;

        startPos.x = startOpen ? Camera.main.pixelWidth : 0;
        endPos.x = !startOpen ? Camera.main.pixelWidth : 0;


        Vector3 botStart = Vector3.zero;
        Vector3 botEnd = Vector3.zero;

        botStart.y = startOpen ? -Camera.main.pixelWidth : 0;
        botEnd.y = !startOpen ? Camera.main.pixelWidth : 0;

        DOTween.To((time) =>
        {
            _cloudLeft.anchoredPosition = Vector3.Lerp(-startPos, -endPos, time);
            _cloudRight.anchoredPosition = Vector3.Lerp(startPos, endPos, time);
            _cloudBot.anchoredPosition = Vector3.Lerp(botStart, -botEnd, time);

        }, 0, 1, duration)
        .OnComplete(() =>
        {
            _cloudLeft.gameObject.SetActive(false);
            _cloudRight.gameObject.SetActive(false);

            if (todoAfter != null)
                todoAfter();
        });
    }

    public void FadeScreen(float startAlpha, float endAlpha, float duration, Action todoAfter = null)
    {
        Color startColor = _image.color;
        startColor.a = startAlpha;

        Color targetColor = startColor;
        targetColor.a = endAlpha;

        DOTween.To((time) =>
        {
            _image.color = Color.Lerp(startColor, targetColor, time);
        }, 0, 1, duration)
        .OnComplete(() =>
        {
            if (todoAfter != null)
                todoAfter();
        });
    }
}
