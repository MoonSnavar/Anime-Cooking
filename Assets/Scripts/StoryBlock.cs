using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBlock : MonoBehaviour
{
    public Action OnStoryTextEnd;
    [SerializeField] private TextObject textObjectsForFunMode;
    [SerializeField] private TextObject[] textObjects;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private float delay;
    private TextObject textObject;
    private int currentString = 0;
    private string lastImagePath;
    private void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("Currentlevel");
        if (currentLevel >= 0)
            textObject = textObjects[currentLevel];
        else
        {
            if (!PlayerPrefs.HasKey("FirsTimeInFunMode"))
            {
                textObject = textObjectsForFunMode;
                PlayerPrefs.SetInt("FirsTimeInFunMode", 1);
            }
            else
            {
                OnStoryTextEnd?.Invoke();
                gameObject.SetActive(false);
                return;
            }
        }

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
