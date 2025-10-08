using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private GameUIManager _gameUIManager => GameUIManager.Instance;
    [SerializeField] GameObject _level;
    private Level _levelScript;
    private readonly WaitForSeconds _timeShowPanel = new WaitForSeconds(0.8f);

    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        Instantiate(_level, transform);
        _levelScript = _level.GetComponentInChildren<Level>();
        _gameUIManager.InitView(_levelScript.LevelNo);
        _gameUIManager.ShowBottomBar(!_levelScript.IsOnBoarding);
        ViewportHandler.Instance.UnitsSize = _levelScript.CameraSize;
        ViewportHandler.Instance.ComputeResolution();
        // AudioManager.PlayLoopSound("MainTheme");
    }
    
    public void LevelCompleted()
    {
        GameData.Instance.SaveLevelData(_levelScript.LevelNo);
        StartCoroutine(ShowCompletedPanel());
    }

    private IEnumerator ShowCompletedPanel()
    {
        yield return _timeShowPanel;
        _gameUIManager.ShowCompletedPanel();
    }
}
