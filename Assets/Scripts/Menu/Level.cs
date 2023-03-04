using System;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelID;
    public Action<int> OnClick;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private TMP_Text textID;

    private void Start()
    {
        textID.text = levelID.ToString();
        int length = PlayerPrefs.GetInt(levelID + "Level", 0);
        for (int i = 0; i < length; i++)
        {
            stars[i].SetActive(true);
        }
    }
    public void Click()
    {
        OnClick?.Invoke(levelID);
    }
}
