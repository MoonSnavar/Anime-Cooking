using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InstantGamesBridge;

public class UITranslator : MonoBehaviour
{
    public List<CustomData> uisDataList = new List<CustomData>();
    [SerializeField] private UITextData uiTextDataRU;
    [SerializeField] private UITextData uiTextDataENG;
    private UITextData currentTextData;

    [System.Serializable]
    public struct CustomData
    {
        public string name;
        public TMP_Text textObject;
    }

    private void Start()
    {
        //определять язык
        if (Bridge.platform.language == "ru")
            currentTextData = uiTextDataRU;
        else
            currentTextData = uiTextDataENG;

        currentTextData = uiTextDataRU; /////////////////////////////////////////////////////////

        foreach (var itemTextElement in uisDataList)
        {
            foreach (var data in currentTextData.uiDataList)
            {
                if (itemTextElement.name == data.name)
                {
                    itemTextElement.textObject.text = data.textStrings;
                    break;
                }
            }
        }
    }

    public string GetTextByName(string name)
    {
        foreach (var data in currentTextData.uiDataList)
        {
            if (name == data.name)
            {
                return data.textStrings;
            }
        }
        return "wrong name";
    }
}
