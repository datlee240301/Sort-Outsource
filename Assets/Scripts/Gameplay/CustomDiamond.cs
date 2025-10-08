using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDiamond : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _mpb;

    [ColorUsage(true, true)]
    [SerializeField] private List<Color> _glowColors;

    private static readonly int GlowColor1 = Shader.PropertyToID("_GlowColor");

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _mpb = new MaterialPropertyBlock();

        // ApplyGlowColor();
    }

    public void ApplyGlowColor(int type)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(GlowColor1, _glowColors[type]);
        _renderer.SetPropertyBlock(_mpb);
    }
}
