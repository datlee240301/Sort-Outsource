using System;
using UnityEngine;

public static class GameEventManager
{
    public static Action<bool> SoundStateChanged;
    public static Action<bool> MusicStateChanged;
    public static Action<bool> VibraStateChanged;
    public static Action UndoMoved;
    public static Action AddBox;
    public static Action UpdateCoins;
    public static Action<int> OnLevelSelected;
    public static Action PurchaseAds;
}
