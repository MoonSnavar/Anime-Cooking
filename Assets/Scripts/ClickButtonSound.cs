using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        audioSource.Play();
    }
}
