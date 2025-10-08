using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public static bool RemoveAds
    {
        get => PlayerPrefs.GetInt("RemoveAds", 0) == 1;
        set => PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0);
    }
    
    public static int Coins
    {
        get => PlayerPrefs.GetInt("Coin", 0);
        set => PlayerPrefs.SetInt("Coin", value);
    }
    
    public static bool Theme0
    {
        get => PlayerPrefs.GetInt("Theme9", 1) == 1;
        set => PlayerPrefs.SetInt("Theme9", value ? 1 : 0);
    }
    
    public static bool Theme1
    {
        get => PlayerPrefs.GetInt("Theme1", 0) == 1;
        set => PlayerPrefs.SetInt("Theme1", value ? 1 : 0);
    }
    
    public static bool Theme2
    {
        get => PlayerPrefs.GetInt("Theme2", 0) == 1;
        set => PlayerPrefs.SetInt("Theme2", value ? 1 : 0);
    }
    
    public static bool Theme3
    {
        get => PlayerPrefs.GetInt("Theme3", 0) == 1;
        set => PlayerPrefs.SetInt("Theme3", value ? 1 : 0);
    }
    
    public static bool Theme4
    {
        get => PlayerPrefs.GetInt("Theme4", 0) == 1;
        set => PlayerPrefs.SetInt("Theme4", value ? 1 : 0);
    }
    
    public static bool Theme5
    {
        get => PlayerPrefs.GetInt("Theme5", 0) == 1;
        set => PlayerPrefs.SetInt("Theme5", value ? 1 : 0);
    }
    
    public static bool Theme6
    {
        get => PlayerPrefs.GetInt("Theme6", 0) == 1;
        set => PlayerPrefs.SetInt("Theme6", value ? 1 : 0);
    }
    
    public static bool Theme7
    {
        get => PlayerPrefs.GetInt("Theme7", 0) == 1;
        set => PlayerPrefs.SetInt("Theme7", value ? 1 : 0);
    }
    
    public static bool Theme8
    {
        get => PlayerPrefs.GetInt("Theme8", 0) == 1;
        set => PlayerPrefs.SetInt("Theme8", value ? 1 : 0);
    }
    
    public static int ThemeEquipped
    {
        get => PlayerPrefs.GetInt("ThemeEquipped", 0);
        set => PlayerPrefs.SetInt("ThemeEquipped", value);
    }
    
    public static int LevelSelected
    {
        get => PlayerPrefs.GetInt("Level_Selected", 1);
        set => PlayerPrefs.SetInt("Level_Selected", value);
    }
    
    public static int Undo
    {
        get => PlayerPrefs.GetInt("Undo", 3);
        set => PlayerPrefs.SetInt("Undo", value);
    }
    
    public static int AddBox
    {
        get => PlayerPrefs.GetInt("AddBox", 2);
        set => PlayerPrefs.SetInt("AddBox", value);
    }
}
