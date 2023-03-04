using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TextData", menuName = "Text Data", order = 51)]
public class TextObject : ScriptableObject
{
    public List<CustomData> stringsDataList = new List<CustomData>();
    
    [System.Serializable]
    public struct CustomData
    {
        [TextArea()] public string textStrings;
        public string imagePath;
        public string name;
    }
}
