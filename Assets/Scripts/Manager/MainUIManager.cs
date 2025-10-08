using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{
    public bool PopupOpened;
    private static SceneTransition _scene => Instance?.GetComponent<SceneTransition>();
    
    public static void LoadScene(string sceneName)
    {
        _scene.PerformTransition(sceneName);
    }
}
