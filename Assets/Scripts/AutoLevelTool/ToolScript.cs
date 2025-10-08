using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour
{
    [System.Serializable]
    public class RootJson
    {
        public List<LevelJson> levels;
    }

    [System.Serializable]
    public class LevelJson
    {
        public int no;
        public List<MapBoxJson> map;
    }

    [System.Serializable]
    public class MapBoxJson
    {
        public List<int> values;
    }
}
