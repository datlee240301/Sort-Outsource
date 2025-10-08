using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private CompletedPanel _completedPanel;
    [SerializeField] private NotEnoughCoinPopup _notEnoughCoinPopup;
    [SerializeField] private ShopPanel _shopPanel;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private GameObject _bottomBar;
    [SerializeField] private Image _bgImage;
    [SerializeField] private Sprite[] _backgroundSprites;
    [Header("Booster Settings")]
    [SerializeField] private Image _undoButton;
    [SerializeField] private Image _addBoxButton;
    [SerializeField] private TextMeshProUGUI _undoText;
    [SerializeField] private TextMeshProUGUI _addBoxText;
    [SerializeField] private Image _undoCost;
    [SerializeField] private Image _addBoxCost;
    [SerializeField] private Sprite[] _boosterSprites;
    
    private int _undoTimeUsed = 0;
    private int _addBoxTimeUsed = 0;

    private void OnEnable()
    {
        GameEventManager.UpdateCoins += UpdateCoins;
        SetupUI();
        UpdateCoins();
    }
    
    private void OnDisable()
    {
        GameEventManager.UpdateCoins -= UpdateCoins;
    }
    
    private void UpdateCoins()
    {
        _coinsText.text = ResourceManager.Coins.ToString();
    }
    
    public void ShowBottomBar(bool show)
    {
        _bottomBar.SetActive(show);
    }
    
    private void SetupUI()
    {
        _pausePanel.gameObject.SetActive(false);
        _completedPanel.gameObject.SetActive(false);
        _notEnoughCoinPopup.gameObject.SetActive(false);
        _shopPanel.gameObject.SetActive(false);
        _bgImage.sprite = _backgroundSprites[ResourceManager.ThemeEquipped];

        UpdateBoosterUI();
    }

    private void UpdateBoosterUI()
    {
        if (ResourceManager.Undo > 0)
        {
            _undoButton.sprite = _boosterSprites[0];
            _undoText.gameObject.SetActive(true);
            _undoText.text = ResourceManager.Undo.ToString();
            _undoCost.gameObject.SetActive(false);
        }
        else
        {
            _undoButton.sprite = _boosterSprites[1];
            _undoText.gameObject.SetActive(false);
            _undoCost.gameObject.SetActive(true);
            _undoCost.sprite = _undoTimeUsed >= 2 ? _boosterSprites[4] : _boosterSprites[2];
        }
        
        if (ResourceManager.AddBox > 0)
        {
            _addBoxButton.sprite = _boosterSprites[0];
            _addBoxText.gameObject.SetActive(true);
            _addBoxText.text = ResourceManager.AddBox.ToString();
            _addBoxCost.gameObject.SetActive(false);
        }
        else
        {
            _addBoxButton.sprite = _boosterSprites[1];
            _addBoxText.gameObject.SetActive(false);
            _addBoxCost.gameObject.SetActive(true);
            _addBoxCost.sprite = _addBoxTimeUsed >= 1 ? _boosterSprites[4] : _boosterSprites[3];
        }
    }
    
    public void InitView(int levelNo)
    {
        _levelText.text = $"Level {levelNo}";
    }
    
    public void OnClickUndo()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.Undo > 0)
        {
            if (LevelController.Instance.MovingCount > 0 || LevelController.Instance.UndoStack.Count <= 0 || LevelController.IsPicking) return;
            ResourceManager.Undo--;
            GameEventManager.UndoMoved?.Invoke();
            UpdateBoosterUI();
        }
        else
        {
            if (_undoTimeUsed >= 2)
            {
                // AdsManager.Instance.ShowRewarded((complete) =>
                // {
                //     ResourceManager.Undo += 3;
                //     UpdateBoosterUI();
                // });
            }
            else
            {
                if (ResourceManager.Coins >= 15)
                {
                    ResourceManager.Coins -= 15;
                    ResourceManager.Undo += 3;
                    _undoTimeUsed++;
                    GameEventManager.UpdateCoins?.Invoke();
                }
                else
                {
                    ShowNotEnoughCoins();
                    return;
                }
            }
            UpdateBoosterUI();
        }
    }

    public void OnClickAddBox()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.AddBox > 0)
        {
            if (LevelController.Instance.MovingCount > 0 || LevelController.Instance.IsCurrentIndexFull()) return;
            ResourceManager.AddBox--;
            GameEventManager.AddBox?.Invoke();
            UpdateBoosterUI();
        }
        else
        {
            if (_addBoxTimeUsed >= 1)
            {
                
            }
            else
            {
                if (ResourceManager.Coins >= 30)
                {
                    ResourceManager.Coins -= 30;
                    ResourceManager.AddBox += 2;
                    _addBoxTimeUsed++;
                    GameEventManager.UpdateCoins?.Invoke();
                }
                else
                {
                    ShowNotEnoughCoins();
                    return;
                }
            }
            UpdateBoosterUI();
        }
    }

    public void OnClickPause()
    {
        AudioManager.PlaySound("Click");
        _pausePanel.gameObject.SetActive(true);
    }
    
    public void ShowCompletedPanel()
    {
        _completedPanel.gameObject.SetActive(true);
    }
    
    private void ShowNotEnoughCoins()
    {
        _notEnoughCoinPopup.gameObject.SetActive(true);
    }
    
    public void OnClickShop()
    {
        AudioManager.PlaySound("Click");
        _shopPanel.gameObject.SetActive(true);
    }
}
