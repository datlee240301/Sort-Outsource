using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _removeAdsButton;
    [SerializeField] private TextMeshProUGUI _removeAdsText;
    [SerializeField] private Sprite _removeAdsOn, _removeAdsOff;
    
    private void OnEnable()
    {
        GameEventManager.UpdateCoins += UpdateCoins;
        GameEventManager.PurchaseAds += UpdateAds;
        UpdateCoins();
        UpdateAds();
    }

    private void Start()
    {
        SetupUI();
    }

    private void OnDisable()
    {
        GameEventManager.UpdateCoins -= UpdateCoins;
        GameEventManager.PurchaseAds -= UpdateAds;
    }
    
    private void SetupUI()
    {
        if (_canvas == null) return;
        float canvasWidth = ((RectTransform)_canvas.transform).rect.width;
        float canvasHeight = ((RectTransform)_canvas.transform).rect.height;
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }
    
    private void UpdateCoins()
    {
        _coinsText.text = ResourceManager.Coins.ToString();
    }
    
    private void UpdateAds()
    {
        _removeAdsButton.interactable = !ResourceManager.RemoveAds;
        _removeAdsButton.image.sprite = ResourceManager.RemoveAds ? _removeAdsOn : _removeAdsOff;
        _removeAdsText.gameObject.SetActive(!ResourceManager.RemoveAds);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    public void WatchAdForCoins()
    {
        AudioManager.PlaySound("Click");
        AdsManager.Instance.ShowRewarded(complete =>
        {
            ResourceManager.Coins += 50;
            GameEventManager.UpdateCoins?.Invoke();
        });
    }
}
