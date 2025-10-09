using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedPanel : MonoBehaviour
{
    [SerializeField] private int _rewardCoins = 20;

    private void OnEnable()
    {
        AudioManager.PlaySound("Completed");
    }

    public void OnClickContinue()
    {
        AudioManager.PlaySound("Click");
        ResourceManager.Coins += _rewardCoins;
        if (ResourceManager.LevelSelected >= 4)
        {
            MainUIManager.LoadScene("Gameplay"); ;
        }
        else
        {
            MainUIManager.LoadScene("Gameplay");
        }
    }

    public void OnClickAds()
    {
        AudioManager.PlaySound("Click");
        
    }
}