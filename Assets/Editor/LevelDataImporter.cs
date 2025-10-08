using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class LevelDataImporter : EditorWindow
{
    private TextAsset jsonFile;

    [MenuItem("Tools/Import Levels From JSON")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataImporter>("Import LevelData");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import Levels from JSON", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        if (GUILayout.Button("Import") && jsonFile != null)
        {
            ImportJson(jsonFile.text);
        }
    }

    private void ImportJson(string json)
    {
        ToolScript.RootJson root = JsonUtility.FromJson<ToolScript.RootJson>(json);

        foreach (var level in root.levels)
        {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

            // Bắt đầu từ LevelNo = 2 trở đi
            levelData.LevelNo = level.no + 1;

            foreach (var box in level.map)
            {
                BoxData boxData = new BoxData();
                foreach (var value in box.values)
                {
                    DiamondData diamond = new DiamondData
                    {
                        Type = value,
                        Position = Vector2.zero // Gán tự động qua OnValidate
                    };
                    boxData.Diamonds.Add(diamond);
                }
                levelData.Boxes.Add(boxData);
            }

            // Tạo path lưu, theo LevelNo đã tăng
            string folderPath = "Assets/ScriptableObject";
            string filePath = $"{folderPath}/Level {levelData.LevelNo}.asset";

            // Kiểm tra và tạo thư mục nếu chưa có
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tránh ghi đè nếu asset đã tồn tại
            if (File.Exists(filePath))
            {
                Debug.LogWarning($"Bỏ qua: Level {levelData.LevelNo} đã tồn tại.");
                continue;
            }

            foreach (var box in levelData.Boxes)
            {
                box.AutoAssignDiamondPositions();
            }
            
            AssetDatabase.CreateAsset(levelData, filePath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
