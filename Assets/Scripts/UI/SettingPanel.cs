using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Toggle[] _toggles;
    
    private void OnEnable()
    {
        Setup();
    }
    
    private void Setup()
    {
        _toggles[0].isOn = AudioManager.IsMusicEnable;
        _toggles[1].isOn = AudioManager.IsSoundEnable;
        _toggles[2].isOn = AudioManager.IsVibrationEnable;
        
        _toggles[0].onValueChanged.AddListener(ToggleMusic);
        _toggles[1].onValueChanged.AddListener(ToggleSound);
        _toggles[2].onValueChanged.AddListener(ToggleVibration);
    }
    
    private void ToggleMusic(bool isOn)
    {
        AudioManager.IsMusicEnable = isOn;
    }
    
    private void ToggleSound(bool isOn)
    {
        AudioManager.IsSoundEnable = isOn;
    }
    
    private void ToggleVibration(bool isOn)
    {
        AudioManager.IsVibrationEnable = isOn;
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Rate()
    {
        //Application.OpenURL("https://play.google.com/store/apps/details?id=com.ahmetozaydin.boxpuzzle");
    }
}
