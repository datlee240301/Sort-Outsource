using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Container : MonoBehaviour
{
    public List<Objecter> Objecters;
    public List<Transform> Positions;
    public bool IsDone;
    public int CurrentIndex = 0;
    [SerializeField] private GameObject _confettiPrefab;
    [SerializeField] private Transform _endPos;
    public Transform EndPos => _endPos;
    [SerializeField] private SpriteRenderer _containerRenderer;
    [SerializeField] private List<Sprite> _containerSprites;
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public Transform Pos4;
    
    private Stack<Objecter> _objecterStack = new Stack<Objecter>();
    public Stack<Objecter> ObjecterStack => _objecterStack;
    private static Objecter _selectedObjecter;
    private static Container _selectedContainer;
    private readonly WaitForSeconds _timeToPlayAnim = new WaitForSeconds(0.3f);

    private void OnValidate()
    {
        Objecters.Clear();
        foreach (Transform child in transform)
        {
            Objecter objecter = child.GetComponent<Objecter>();
            if (objecter != null)
            {
                Objecters.Add(objecter);
            }
        }
    }

    private void Start()
    {
        Setup();
    }

    public void InsertTransform()
    {
        switch (CurrentIndex)
        {
            case 1:
                Positions.Insert(0, Pos4);
                _containerRenderer.sprite = _containerSprites[0];
                break;
            case 2:
                Positions.Insert(0, Pos3);
                _containerRenderer.sprite = _containerSprites[1];
                break;
            case 3:
                Positions.Insert(0, Pos2);
                _containerRenderer.sprite = _containerSprites[2];
                break;
            case 4:
                Positions.Insert(0, Pos1);
                _containerRenderer.sprite = _containerSprites[3];
                break;
        }
    }

    private void Setup()
    {
        for (int i = Objecters.Count - 1; i >= 0; i--)
        {
            Objecter objecter = Objecters[i];
            _objecterStack.Push(objecter);
        }
        IsDone = false;
    }

    private void OnMouseDown()
    {
        if (!IsDone && !MainUIManager.Instance.PopupOpened)
        {
            LevelController.Instance.HandleContainerClick(this);
        }
    }

    public void CheckHidden()
    {
        if (Objecters.Count > 0 && Objecters[0].IsHidden)
        {
            Objecters[0].Reveal();
        }
    }
    
    public bool AllObjectersSameType()
    {
        if (Objecters.Count < 4) return false;
        var firstType = Objecters[0].Type;
        foreach (var obj in Objecters)
        {
            if (obj.Type != firstType || obj.IsHidden)
                return false;
        }
        return true;
    }
    
    public void CheckCompletion()
    {
        if (Objecters.Count == 4 && AllObjectersSameType())
        {
            IsDone = true;
            AudioManager.PlayDelaySound("TubeDone");
            PlayConfetti();
        }
        else
        {
            IsDone = false;
        }
    }
    
    public void PlayConfetti()
    {
        StartCoroutine(PlayAnim());
    }

    private IEnumerator PlayAnim()
    {
        yield return _timeToPlayAnim;
        _confettiPrefab.SetActive(true);
        AudioManager.LightFeedback();
    }
}