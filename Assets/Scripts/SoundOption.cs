using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider bGMSlider;
    public Slider sESlider;
    public float bgmSound;
    public float seSound;

    private void Start()
    {
        bGMSlider.onValueChanged.AddListener(SetBGM);
        sESlider.onValueChanged.AddListener(SetSE);
    }

    public void SetBGM(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("BGM_Volume", volume);
    }

    public void SetSE(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("SE_Volume", volume);
    }
}