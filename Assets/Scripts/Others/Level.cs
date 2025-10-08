using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int LevelNo => LevelData[ResourceManager.LevelSelected - 1].LevelNo;
    
    public bool IsOnBoarding;
    public float CameraSize => LevelData[ResourceManager.LevelSelected - 1].CameraSize;
    public LevelData[] LevelData;
    public LevelController LevelController;
    public OnBoardingLevel OnBoardingLevel;

    private void Start()
    {
        int currentLevel = ResourceManager.LevelSelected;
        if (currentLevel == 1)
        {
            IsOnBoarding = true;
        }
        for (int i = 0; i < LevelData[currentLevel - 1].Boxes.Count; i++)
        {
            GameObject boxObject = Instantiate(LevelController.ContainerPrefab, LevelController.ContainerParent);
            if (currentLevel == 1)
            {
                if (i == 0) OnBoardingLevel.Box1 = boxObject.GetComponent<Collider2D>();
                if (i == 1) OnBoardingLevel.Box2 = boxObject.GetComponent<Collider2D>();
                GameUIManager.Instance.ShowBottomBar(false);
            }
            Container container = boxObject.GetComponent<Container>();
            container.Objecters = new List<Objecter>();
            LevelController.Containers.Add(container);

            for (int j = 0; j < LevelData[currentLevel - 1].Boxes[i].Diamonds.Count; j++)
            {
                Vector2 position = LevelData[currentLevel - 1].Boxes[i].Diamonds[j].Position;
                int type = LevelData[currentLevel - 1].Boxes[i].Diamonds[j].Type;
                bool isHidden = LevelData[currentLevel - 1].Boxes[i].Diamonds[j].IsHidden;

                GameObject diamondObject = Instantiate(LevelController.DiamondPrefab, boxObject.transform);
                Objecter diamond = diamondObject.GetComponent<Objecter>();
                diamond.Type = type;
                diamond.IsHidden = isHidden;
                diamond.transform.localPosition = position;

                container.Objecters.Add(diamond);
            }
        }
    }
}
