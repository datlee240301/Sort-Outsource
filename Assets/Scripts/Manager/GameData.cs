using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    private const string SaveFileName = "GameData.json";
    public int LevelNumber = 10; // <== Số level mặc định

    [Serializable]
    public class LevelData
    {
        public int LevelID;
        public bool IsCompleted;
        public bool IsUnlocked;
    }

    [Serializable]
    public class Data
    {
        public List<LevelData> Levels = new List<LevelData>();
    }

    private Data _saveData;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstRun"))
        {
            ResetGameData();
            PlayerPrefs.SetInt("FirstRun", 1);
        }

        LoadData();
    }

    private string GetSaveFilePath()
    {
#if UNITY_EDITOR
        return Path.Combine(Application.dataPath, SaveFileName);
#else
        return Path.Combine(Application.persistentDataPath, SaveFileName);
#endif
    }

    public void SaveLevelData(int levelID)
    {
        var level = _saveData.Levels.Find(l => l.LevelID == levelID);
        if (level == null)
        {
            level = new LevelData { LevelID = levelID, IsCompleted = true, IsUnlocked = true };
            _saveData.Levels.Add(level);
        }
        else
        {
            level.IsCompleted = true;
        }

        var nextLevel = _saveData.Levels.Find(l => l.LevelID == levelID + 1);
        if (nextLevel != null)
        {
            nextLevel.IsUnlocked = true;
        }

        ResourceManager.LevelSelected++;
        string json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(GetSaveFilePath(), json);
    }

    public List<LevelData> GetAllLevels()
    {
        if (_saveData == null)
            LoadData();

        return _saveData.Levels;
    }

    private void LoadData()
    {
        if (File.Exists(GetSaveFilePath()))
        {
            string json = File.ReadAllText(GetSaveFilePath());
            _saveData = JsonUtility.FromJson<Data>(json);
        }
    }

    private void ResetGameData()
    {
        if (File.Exists(GetSaveFilePath()))
        {
            File.Delete(GetSaveFilePath());
        }

        _saveData = new Data();

        for (int i = 1; i <= LevelNumber; i++)
        {
            _saveData.Levels.Add(new LevelData
            {
                LevelID = i,
                IsCompleted = false,
                IsUnlocked = (i == 1)
            });
        }

        string json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(GetSaveFilePath(), json);
    }
}
