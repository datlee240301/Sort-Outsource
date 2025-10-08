using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _levelImage;
    [SerializeField] private Sprite[] _levelSprites;
    [SerializeField] private Button _levelButton;
    
    public void Setup(int level, bool isUnlocked)
    {
        _levelText.text = level.ToString();
        
        _levelImage.sprite = isUnlocked ? _levelSprites[0] : _levelSprites[2];
        if (level == ResourceManager.LevelSelected)
        {
            _levelImage.sprite = _levelSprites[1];
        }
        _levelText.gameObject.SetActive(isUnlocked);
        _levelButton.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Click");
            if (!isUnlocked) return;
            GameEventManager.OnLevelSelected?.Invoke(level);
        });
    }
}
