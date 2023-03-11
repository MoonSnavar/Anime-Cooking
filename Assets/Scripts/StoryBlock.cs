using InstantGamesBridge;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBlock : MonoBehaviour
{
    public Action OnStoryTextEnd;
    [SerializeField] private TextObject textObjectsForFunModeRU;
    [SerializeField] private TextObject textObjectsForFunModeENG;
    [SerializeField] private TextObject[] textObjectsRU;
    [SerializeField] private TextObject[] textObjectsENG;
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
        {
            if (currentLevel == 0 || currentLevel == 1)
            {
                if (Bridge.platform.language == "ru")
                    textObject = textObjectsRU[currentLevel];
                else
                    textObject = textObjectsENG[currentLevel];
            }
            else
            {
                int textDataIndex = 1;
                for (int i = 4; i <= 40; i+=4)
                {
                    textDataIndex++;
                    if (currentLevel == i)
                    {
                        if (Bridge.platform.language == "ru")
                            textObject = textObjectsRU[textDataIndex];
                        else
                            textObject = textObjectsENG[textDataIndex];
                    }
                }
            }
            if (textObject == null)
            {
                OnStoryTextEnd?.Invoke();
                gameObject.SetActive(false);
                return;
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey("FirsTimeInFunMode"))
            {
                if (Bridge.platform.language == "ru")
                    textObject = textObjectsForFunModeRU;
                else
                    textObject = textObjectsForFunModeENG;
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
