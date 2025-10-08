using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{
    [SerializeField] private Image[] _checkMarks;
    [SerializeField] private Image[] _buttons;
    [SerializeField] private Sprite _themeOwned;
    [SerializeField] private Sprite _themeLocked;
    [SerializeField] private Sprite[] _buttonSprites;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private bool[] _isUnlocked;

    private void OnEnable()
    {
        GameEventManager.UpdateCoins += UpdateCoins;
        Setup();
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

    private void Setup()
    {
        _isUnlocked = new bool[]
        {
            ResourceManager.Theme0, ResourceManager.Theme1, ResourceManager.Theme2, 
            ResourceManager.Theme3, ResourceManager.Theme4, ResourceManager.Theme5, 
            ResourceManager.Theme6, ResourceManager.Theme7, ResourceManager.Theme8,
        };

        for (int i = 0; i < _checkMarks.Length; i++)
        {
            if (!_isUnlocked[i])
            {
                _checkMarks[i].gameObject.SetActive(true);
                _checkMarks[i].sprite = _themeLocked;
                _buttons[i].sprite = _buttonSprites[2];
            }
            else
            {
                _checkMarks[i].gameObject.SetActive(false);
                _buttons[i].sprite = _buttonSprites[0];
            }
        }
        
        _checkMarks[ResourceManager.ThemeEquipped].gameObject.SetActive(true);
        _checkMarks[ResourceManager.ThemeEquipped].sprite = _themeOwned;
        _buttons[ResourceManager.ThemeEquipped].sprite = _buttonSprites[1];
    }

    public void OnClickSelectTheme(int themeIndex)
    {
        AudioManager.PlaySound("Click");

        if (_isUnlocked[themeIndex])
        {
            if (ResourceManager.ThemeEquipped == themeIndex) return;

            ResourceManager.ThemeEquipped = themeIndex;
        }
        else
        {
            if (ResourceManager.Coins >= 500)
            {
                ResourceManager.Coins -= 500;
                GameEventManager.UpdateCoins();
                SetThemeUnlocked(themeIndex);
            }
            else
            {
                HomeUIManager.Instance.ShowNotEnoughCoins();
            }
        }
        Setup();
    }
    
    private void SetThemeUnlocked(int index)
    {
        switch (index)
        {
            case 0: ResourceManager.Theme0 = true; break;
            case 1: ResourceManager.Theme1 = true; break;
            case 2: ResourceManager.Theme2 = true; break;
            case 3: ResourceManager.Theme3 = true; break;
            case 4: ResourceManager.Theme4 = true; break;
            case 5: ResourceManager.Theme5 = true; break;
            case 6: ResourceManager.Theme6 = true; break;
            case 7: ResourceManager.Theme7 = true; break;
            case 8: ResourceManager.Theme8 = true; break;
        }
    }
}
