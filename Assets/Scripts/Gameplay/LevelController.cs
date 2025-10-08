using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    public List<Container> Containers;
    public Transform ContainerParent;
    public GameObject ContainerPrefab;
    public GameObject DiamondPrefab;
    [SerializeField] private float _arcHeight = 3f; // Chiều cao của vòng cung
    [SerializeField] private Ease _easeType;
    [SerializeField] private float _timeToMove = 0.4f;
    [SerializeField] private float _multiplier = 1.5f;
    
    private static bool _isPicking;
    public static bool IsPicking => _isPicking;
    private bool _isAlreadyAddedContainers = false;
    private Container _selectedContainer;
    private Objecter _selectedObjecter;
    private Stack<MoveStep> _undoStack = new Stack<MoveStep>();
    public Stack<MoveStep> UndoStack => _undoStack;
    private GameObject _newContainer;
    private bool _isLevelCompleted = false;
    private int _movingCount = 0;
    public int MovingCount => _movingCount;

    private void Start()
    {
        _isPicking = false;
        _movingCount = 0;
        GameEventManager.UndoMoved += UndoMoved;
        GameEventManager.AddBox += AddMoreContainers;
    }
    
    private void OnDestroy()
    {
        GameEventManager.UndoMoved -= UndoMoved;
        GameEventManager.AddBox -= AddMoreContainers;
    }
    
    private void UndoMoved()
    {
        if (_undoStack.Count == 0 || _isPicking || _movingCount > 0) return;
        
        MoveStep step = _undoStack.Pop();
        
        for (int i = step.Moves.Count - 1; i >= 0; i--)
        {
            var move = step.Moves[i];
            
            move.TargetContainer.Objecters.Remove(move.Objecter);
            move.TargetContainer.ObjecterStack.Pop();
            move.TargetContainer.IsDone = false;
            
            move.SourceContainer.Objecters.Add(move.Objecter);
            move.SourceContainer.ObjecterStack.Push(move.Objecter);
            
            int lastIndex = move.SourceContainer.Positions.Count - 
                            move.SourceContainer.Objecters.Count;
            Vector3 targetPos = move.SourceContainer.Positions[lastIndex].position;
            MoveObjecter(move.Objecter, move.TargetContainer, move.SourceContainer, targetPos);
        }
    }
    
    private void AddMoreContainers()
    {
        if (_movingCount > 0) return;
        if (!_isAlreadyAddedContainers)
        {
            _newContainer = Instantiate(ContainerPrefab, ContainerParent);
            var containerComponent = _newContainer.GetComponent<Container>();
            containerComponent.CurrentIndex = 1;
            containerComponent.Positions.Clear();
            containerComponent.InsertTransform();
            Containers.Add(containerComponent);
            _isAlreadyAddedContainers = true;
        }
        else
        {
            // Add thêm transform mới vào List của container được add
            var containerComponent = _newContainer.GetComponent<Container>();
            if (containerComponent.CurrentIndex < 4)
            {
                containerComponent.CurrentIndex++;
                containerComponent.InsertTransform();
            }
        }
    }

    public bool IsCurrentIndexFull()
    {
        if (_newContainer == null) return false;
        return _newContainer.GetComponent<Container>().CurrentIndex >= 4;
    }

    public void HandleContainerClick(Container container)
    {
        if (container.Objecters.Count > 0)
        {
            if (container.Objecters[0].IsHidden)
            {
                return;
            }
        }
        if (!_isPicking)
        {
            if (container.Objecters.Count == 0) return;
            _selectedObjecter = container.ObjecterStack.Pop();
            _selectedContainer = container;
            _isPicking = true;
            container.Objecters.Remove(_selectedObjecter);
            MoveObjecter(_selectedObjecter, null, container, container.EndPos.position);
        }
        else
        {
            if (_selectedContainer == container)
            {
                AudioManager.PlaySound("Diamond");
                _selectedContainer.ObjecterStack.Push(_selectedObjecter);
                _selectedContainer.Objecters.Insert(0, _selectedObjecter);
                int lastIndex = _selectedContainer.Positions.Count - _selectedContainer.Objecters.Count;
                MoveObjecter(_selectedObjecter, null, _selectedContainer, _selectedContainer.Positions[lastIndex].position);
                _isPicking = false;
                _selectedObjecter = null;
                _selectedContainer = null;
            }
            else
            {
                if (container.Objecters.Count < container.Positions.Count && (container.Objecters.Count == 0 || container.ObjecterStack.Peek().Type == _selectedObjecter.Type))
                {
                    MoveStep step = new MoveStep();
                    MoveObjecterToContainer(_selectedObjecter, container, step);

                    while (_selectedContainer.ObjecterStack.Count > 0 && 
                           container.Objecters.Count < container.Positions.Count && 
                           _selectedContainer.ObjecterStack.Peek().Type == _selectedObjecter.Type &&
                           !_selectedContainer.ObjecterStack.Peek().IsHidden)
                    {
                        Objecter nextObjecter = _selectedContainer.ObjecterStack.Pop();
                        _selectedContainer.Objecters.Remove(nextObjecter);
                        MoveObjecterToContainer(nextObjecter, container, step);
                    }

                    _undoStack.Push(step);
                    _isPicking = false;
                    _selectedObjecter = null;
                    _selectedContainer = null;
                }
                else
                {
                    _selectedContainer.ObjecterStack.Push(_selectedObjecter);
                    _selectedContainer.Objecters.Insert(0, _selectedObjecter);
                    int lastIndex = _selectedContainer.Positions.Count - _selectedContainer.Objecters.Count;
                    AudioManager.PlaySound("Diamond");
                    MoveObjecter(_selectedObjecter, null, _selectedContainer, _selectedContainer.Positions[lastIndex].position);
                    
                    _selectedObjecter = container.ObjecterStack.Pop();
                    _selectedContainer = container;
                    container.Objecters.Remove(_selectedObjecter);
                    MoveObjecter(_selectedObjecter, null, container, container.EndPos.position);
                }
            }
        }
    }
    
    private void MoveObjecterToContainer(Objecter objecter, Container target, MoveStep step)
    {
        target.ObjecterStack.Push(objecter);
        target.Objecters.Add(objecter);
        target.CheckCompletion();
        _selectedContainer.CheckCompletion();
        int lastIndex = target.Positions.Count - target.Objecters.Count;
        MoveObjecter(objecter, _selectedContainer, target, target.Positions[lastIndex].position);
        
        step.Moves.Add(new MoveStep.ObjecterMove {
            Objecter = objecter,
            SourceContainer = _selectedContainer,
            TargetContainer = target
        });
        
        _selectedContainer.CheckHidden();
    }
    
    private void MoveObjecter(Objecter objecter, Container sourceContainer, Container targetContainer, Vector3 finalPosition)
    {
        _movingCount++;
        if (targetContainer != null && objecter.transform.parent != targetContainer.transform)
        {
            objecter.transform.SetParent(targetContainer.transform, true);
        }

        if (sourceContainer == null || targetContainer == null || sourceContainer == targetContainer)
        {
            objecter.transform.DOMove(finalPosition, _timeToMove).SetEase(_easeType).OnComplete(() =>
            {
                _movingCount--;
            });
        }
        else
        {
            AudioManager.PlayDelaySound("Diamond");
            Vector3 sourceEndPos = sourceContainer.EndPos.position;
            Vector3 targetEndPos = targetContainer.EndPos.position;
            Vector3 midPoint = (sourceEndPos + targetEndPos) / 2 + Vector3.up * _arcHeight;

            Vector3[] path = new Vector3[]
            {
                sourceEndPos,
                midPoint,
                targetEndPos,
                finalPosition
            };

            objecter.transform.DOPath(path, _timeToMove * _multiplier, PathType.CatmullRom).SetEase(_easeType).OnComplete(
                () =>
                {
                    _movingCount--;
                    CheckHidden(sourceContainer);
                    CheckWin();
                });
        }
    }
    
    private void CheckHidden(Container container)
    {
        if (container.Objecters.Count == 0) return;
        container.CheckHidden();
    }
    
    private void CheckWin()
    {
        if (_isLevelCompleted) return;

        bool allDone = Containers
            .Where(c => c.Objecters.Count > 0)
            .All(c => c.IsDone);

        if (allDone)
        {
            _isLevelCompleted = true;
            GameController.Instance.LevelCompleted();
        }
    }
}

public class MoveStep
{
    public struct ObjecterMove
    {
        public Objecter Objecter;
        public Container SourceContainer;
        public Container TargetContainer;
    }
    
    public List<ObjecterMove> Moves = new(4);
}