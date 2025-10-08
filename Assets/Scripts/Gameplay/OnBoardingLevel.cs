using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class OnBoardingLevel : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _notice;
    public Collider2D Box1;
    public Collider2D Box2;
    private bool _animPlayed;
    private float _timeToMove = 0.5f;
    private readonly Vector2 _startPos = new Vector2(-1.05f, 2.2f);
    private readonly Vector2 _endPos = new Vector2(2.75f, 2.2f);

    private void Start()
    {
        if (_level.IsOnBoarding)
        {
            Box2.enabled = false;
            _hand.transform.position = _startPos;
        }
        else
        {
            _hand.SetActive(false);
            _notice.SetActive(false);
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit == Box1)
            {
                if (!_animPlayed)
                {
                    _hand.transform.DOMoveX(_endPos.x, _timeToMove);
                    _animPlayed = true;
                }
                Box2.enabled = true;
                Box1.enabled = false;
            }
        }
    }
}
