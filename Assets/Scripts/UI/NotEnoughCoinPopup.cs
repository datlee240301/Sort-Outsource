using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NotEnoughCoinPopup : MonoBehaviour
{
    private readonly Vector3 _targetScale = new Vector3(1.4f, 1.4f, 1.4f);
    private readonly WaitForSeconds _timeOff = new WaitForSeconds(1.5f);
    private float _timeToShow = 0.4f;
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(_targetScale, _timeToShow)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                StartCoroutine(TurnOff());
            });
    }
    
    private IEnumerator TurnOff()
    {
        yield return _timeOff;
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }
}
