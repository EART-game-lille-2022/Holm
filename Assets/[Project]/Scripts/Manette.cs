using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manette : MonoBehaviour
{
    [SerializeField] RectTransform _hidePanelRect;
    [SerializeField] RectTransform _controlerImageRect;

    void Start()
    {
        _controlerImageRect.localScale = Vector3.zero;
        _controlerImageRect.DOScale(1f, .2f);
        _controlerImageRect.DOShakeAnchorPos(1f, 20, 55)
        .OnComplete(() =>
        {
            _hidePanelRect.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 2)
            .OnComplete(() => StartCoroutine(SceneSwitch()));
        });
    }

    IEnumerator SceneSwitch()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
