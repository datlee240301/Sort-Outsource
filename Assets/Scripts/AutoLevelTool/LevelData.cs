using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int LevelNo;
    public int CameraSize;
    public List<BoxData> Boxes = new List<BoxData>();
    
    private void OnValidate()
    {
        foreach (var box in Boxes)
        {
            box.AutoAssignDiamondPositions();
        }
    }
}

[Serializable]
public class DiamondData
{
    public int Type;
    public bool IsHidden;
    public Vector2 Position;
}

[Serializable]
public class BoxData
{
    public List<DiamondData> Diamonds = new List<DiamondData>();

    public void AutoAssignDiamondPositions()
    {
        Vector2[] positions = new Vector2[]
        {
            new Vector2(0, 2.75f),
            new Vector2(0, 1.075f),
            new Vector2(0, -0.6f),
            new Vector2(0, -2.275f)
        };

        int count = Mathf.Min(Diamonds.Count, 4);
        for (int i = 0; i < count; i++)
        {
            // Ưu tiên viên cuối nằm dưới cùng => index ngược
            int reversedIndex = Diamonds.Count - 1 - i;
            Diamonds[reversedIndex].Position = positions[3 - i];
        }
    }
}
