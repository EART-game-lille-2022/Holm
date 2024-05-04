using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manette : MonoBehaviour
{
    [SerializeField] RectTransform _rect;

    void Start()
    {
        _rect.localScale = Vector3.zero;
        _rect.DOScale(1f, .2f);
        _rect.DOShakeAnchorPos(1f, 20, 55)
        .OnComplete(() =>
        {
            _rect.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1)
            .OnComplete(() => StartCoroutine(SceneSwitch()));
        });
    }

    IEnumerator SceneSwitch()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
