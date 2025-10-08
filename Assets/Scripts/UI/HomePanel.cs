using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private RectTransform _content;
    private readonly float _contentHeight = 20f;
    
    private void OnEnable()
    {
        GameEventManager.UpdateCoins += UpdateCoins;
        UpdateCoins();
    }
    
    private void OnDisable()
    {
        GameEventManager.UpdateCoins -= UpdateCoins;
    }

    private void Start()
    {
        SpawnLine();
        SpawnLevel();
        ScrollToLastUnlockedLevel();
    }

    private void UpdateCoins()
    {
        _coinsText.text = ResourceManager.Coins.ToString();
    }
    
    private void SpawnLine()
    {
        (float x, float rotZ)[] pattern = new (float x, float rotZ)[]
        {
            (130f,  52f),
            (-130f, 52f),
            (-130f, -52f),
            (130f, -52f)
        };

        List<GameData.LevelData> allLevels = GameData.Instance.GetAllLevels();

        int lastIndex = allLevels.Count - 1;
        if (lastIndex <= 0)
            return;

        float yStart = 170f;

        for (int i = 0; i < lastIndex; i++)
        {
            var (x, rotZ) = pattern[i % pattern.Length];
            float y = yStart + (i * 200f);

            GameObject lineObj = Instantiate(_linePrefab, _container);
            lineObj.transform.localPosition = new Vector3(x, y, 0f);
            lineObj.transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    private void SpawnLevel()
    {
        float y = 0f;
        float[] xPattern = { 260f, 0f, -260f, 0f };

        List<GameData.LevelData> allLevels = GameData.Instance.GetAllLevels();

        for (int i = 0; i < allLevels.Count; i++)
        {
            var levelData = allLevels[i];

            float x = xPattern[i % xPattern.Length];
            Vector3 localPosition = new Vector3(x, y, 0f);

            GameObject levelObj = Instantiate(_levelPrefab, _container);
            levelObj.transform.localPosition = localPosition;

            levelObj.GetComponent<LevelUI>().Setup(levelData.LevelID, levelData.IsUnlocked);

            y += 200f;
        }

        _content.sizeDelta = new Vector2(0, _contentHeight + y);
    }

    private void ScrollToLastUnlockedLevel()
    {
        int lastUnlockedIndex = ResourceManager.LevelSelected;
        if (lastUnlockedIndex <= 0) return;
        if (lastUnlockedIndex >= 4)
        {
            int y = -100;
            var result = y - (lastUnlockedIndex - 4) * 200;
            _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, result);
        }
    }
}
