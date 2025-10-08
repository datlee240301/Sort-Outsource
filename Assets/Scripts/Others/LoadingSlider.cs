using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _time;
    private float currentTime;
    private bool isLoading;
    
    private void Awake()
    {
        _slider.value = 0f;
        StartLoading();
    }
    
    private void StartLoading()
    {
        currentTime = 0f;
        isLoading = true;
    }

    private void Update()
    {
        if (isLoading)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / _time;
            
            _slider.value = Mathf.Clamp01(progress);
            
            if (progress >= 1f)
            {
                isLoading = false;
                MainUIManager.LoadScene("Home");
            }
        }
    }
}