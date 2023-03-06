using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UITextData", menuName = "UI Text Data", order = 51)]
public class UITextData : ScriptableObject
{
    public List<CustomData> uiDataList = new List<CustomData>();

    [System.Serializable]
    public struct CustomData
    {
        public string name;
        [TextArea()] public string textStrings;        
    }
}
