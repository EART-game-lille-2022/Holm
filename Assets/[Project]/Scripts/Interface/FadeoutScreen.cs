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

    void Awake()
    {
        instance = this;
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
            if(todoAfter != null)
                todoAfter();
        });
    }
}
