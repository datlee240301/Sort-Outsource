using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    [SerializeField] private Toggle[] _tabToggles;
    [SerializeField] private Image[] _tabImages;
    [SerializeField] private Sprite[] _tabSprites;
    [SerializeField] private RectTransform[] _tabContents;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _tabUI;
    [SerializeField] private float _timeMove = 0.3f;
    [SerializeField] private GameObject[] _tabIcons;
    
    void Start()
    {
        float canvasWidth = ((RectTransform)_canvas.transform).rect.width;
        float canvasHeight = ((RectTransform)_canvas.transform).rect.height;
        foreach (var item in _tabContents)
        {
            item.sizeDelta = new Vector2(canvasWidth, canvasHeight);
        }
        _tabContents[0].anchoredPosition = new Vector2(-canvasWidth, 0);
        _tabContents[1].anchoredPosition = new Vector2(0, 0);
        _tabContents[2].anchoredPosition = new Vector2(canvasWidth, 0);
        
        for (int i = 0; i < _tabToggles.Length; i++)
        {
            int index = i;
            _tabToggles[i].onValueChanged.AddListener((isOn) =>
            {
                AudioManager.PlaySound("Click");
                if (isOn)
                {
                    ShowTab(index);
                }
            });
        }
        ShowTab(1);
    }

    void ShowTab(int index)
    {
        _tabUI.DOKill();
        float targetX = -_tabContents[index].anchoredPosition.x;
        _tabUI.DOAnchorPosX(targetX, _timeMove);

        for (int i = 0; i < _tabToggles.Length; i++)
        {
            _tabImages[i].sprite = _tabToggles[i].isOn ? _tabSprites[1] : _tabSprites[0];
        }

        if (index == 0)
        {
            _tabIcons[0].SetActive(true);
            _tabIcons[1].SetActive(false);
        }
        else
        {
            _tabIcons[0].SetActive(false);
            _tabIcons[1].SetActive(true);
        }
    }
}
