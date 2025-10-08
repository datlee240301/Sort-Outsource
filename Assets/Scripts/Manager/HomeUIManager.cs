using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIManager : Singleton<HomeUIManager>
{
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] private NotEnoughCoinPopup _notEnoughCoinPopup;
    [SerializeField] private RectTransform _tabWidth;
    [SerializeField] private Canvas _canvas;

    private void OnEnable()
    {
        SetupUI();
        GameEventManager.OnLevelSelected += OnLevelSelected;
    }
    
    private void OnDisable()
    {
        GameEventManager.OnLevelSelected -= OnLevelSelected;
    }
    
    private void SetupUI()
    {
        _settingPanel.gameObject.SetActive(false);
        _notEnoughCoinPopup.gameObject.SetActive(false);
        float canvasWidth = ((RectTransform)_canvas.transform).rect.width;
        float canvasHeight = ((RectTransform)_canvas.transform).rect.height;
        _tabWidth.sizeDelta = new Vector2(canvasWidth * 3f, canvasHeight);
    }

    public void OnClickPlay()
    {
        AudioManager.PlaySound("Click");
        MainUIManager.LoadScene("Gameplay");
    }
    
    public void OnClickSettings()
    {
        AudioManager.PlaySound("Click");
        _settingPanel.gameObject.SetActive(true);
    }
    
    public void ShowNotEnoughCoins()
    {
        _notEnoughCoinPopup.gameObject.SetActive(true);
    }
    
    private void OnLevelSelected(int level)
    {
        ResourceManager.LevelSelected = level;
        MainUIManager.LoadScene("Gameplay");
    }
}
