using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroupBGM;
    [SerializeField] private AudioMixerGroup audioMixerGroupSFX;

    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BGMMusic"))
        {
            sliderBGM.value = PlayerPrefs.GetFloat("BGMMusic");
            ChangeBGMMusic(sliderBGM);
        }
        else
        {
            sliderBGM.value = -10;
            ChangeBGMMusic(sliderBGM);
        }
        if (PlayerPrefs.HasKey("SFXMusic"))
        {
            sliderSFX.value = PlayerPrefs.GetFloat("SFXMusic");
            ChangeSFXMusic(sliderSFX);
        }
        else
        {
            sliderSFX.value = -10;
            ChangeSFXMusic(sliderSFX);
        }
    }
    public void ChangeBGMMusic(Slider slider)
    {
        audioMixerGroupBGM.audioMixer.SetFloat("BGMVolume",slider.value);
        PlayerPrefs.SetFloat("BGMMusic", slider.value);
    }
    public void ChangeSFXMusic(Slider slider)
    {
        audioMixerGroupBGM.audioMixer.SetFloat("SFXVolume", slider.value);
        PlayerPrefs.SetFloat("SFXMusic", slider.value);
    }
}
