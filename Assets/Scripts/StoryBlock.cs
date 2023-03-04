using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBlock : MonoBehaviour
{
    public Action OnStoryTextEnd;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TextObject textObject;
    [SerializeField] private float delay;
    private int currentString = 0;
    private string lastImagePath;
    private void Start()
    {
        lastImagePath = textObject.stringsDataList[currentString].imagePath;
        characterImage.sprite = Resources.Load<Sprite>(textObject.stringsDataList[currentString].imagePath);
        nameText.text = textObject.stringsDataList[currentString].name;
        StartCoroutine(c_Output(textObject.stringsDataList[currentString].textStrings, delay));        
    }

    IEnumerator c_Output(string str, float delay)
    {
        foreach (var sym in str)
        {
            text.text += sym;

            yield return new WaitForSeconds(delay);
        }
    }

    public void NextText()
    {
        if (text.text != textObject.stringsDataList[currentString].textStrings)
        {
            StopAllCoroutines();
            text.text = textObject.stringsDataList[currentString].textStrings;
        }
        else
        {

            if (currentString + 1 < textObject.stringsDataList.Count)
            {
                currentString++;
                StopAllCoroutines();
                text.text = "";
                nameText.text = textObject.stringsDataList[currentString].name;
                StartCoroutine(c_Output(textObject.stringsDataList[currentString].textStrings, delay));
                if (lastImagePath != textObject.stringsDataList[currentString].imagePath)
                {
                    characterImage.sprite = Resources.Load<Sprite>(textObject.stringsDataList[currentString].imagePath);
                    lastImagePath = textObject.stringsDataList[currentString].imagePath;
                }
            }
            else
            {
                OnStoryTextEnd?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
