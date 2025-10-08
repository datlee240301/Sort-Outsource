using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objecter : MonoBehaviour
{
    public int Type;
    public bool IsHidden;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _hiddenSprite;
    [SerializeField] private Sprite[] _typeSprites;

    private void Start()
    {
        _spriteRenderer.sprite = IsHidden ? _hiddenSprite : _typeSprites[Type];
    }

    public void Reveal()
    {
        IsHidden = false;
        _spriteRenderer.sprite = _typeSprites[Type];
    }
}
