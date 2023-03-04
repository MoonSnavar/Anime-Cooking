using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool timerOn = false;
    public Action OnTimerEnd;
    [SerializeField] private LevelCharacteristics levelCharacteristics;
    [SerializeField] private TMP_Text timerText;

    private float time;
    private float timeLeft = 0f;

    private void Start()
    {
        time = 60 + (levelCharacteristics.levelDifficult * 6);


        timeLeft = time;
        timerOn = false;
    }

    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                timeLeft = time;
                timerOn = false;
                OnTimerEnd?.Invoke();
            }
        }
    }

    private void UpdateTimeText()
    {
        if (timeLeft < 0)
            timeLeft = 0;

        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
