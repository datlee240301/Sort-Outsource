using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIManager.Instance.PopupOpened = true;
    }
    
    public void OnClickHome()
    {
        AudioManager.PlaySound("Click");
        MainUIManager.Instance.PopupOpened = false; 
        MainUIManager.LoadScene("Home");
    }

    public void OnClickContinue()
    {
        AudioManager.PlaySound("Click");
        MainUIManager.Instance.PopupOpened = false;
        gameObject.SetActive(false);
    }

    public void OnClickRestart()
    {
        AudioManager.PlaySound("Click");
        if (ResourceManager.LevelSelected >= 4)
        {
            MainUIManager.Instance.PopupOpened = false;
            MainUIManager.LoadScene("Gameplay"); 
        }
        else
        {
            MainUIManager.Instance.PopupOpened = false;
            MainUIManager.LoadScene("Gameplay");
        }
    }
}