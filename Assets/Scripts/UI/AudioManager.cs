using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource clickSound;
    [SerializeField]
    private AudioSource progressionSound;
    [SerializeField]
    private AudioSource failSound;
    [SerializeField]
    private AudioSource music;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Music_Volume"))
            PlayerPrefs.SetInt("Music_Volume", 100);
        if (!PlayerPrefs.HasKey("UI_Volume"))
            PlayerPrefs.SetInt("UI_Volume", 100);

        ChangeMusicVolume(PlayerPrefs.GetInt("Music_Volume"));
        ChangeUiVolume(PlayerPrefs.GetInt("UI_Volume"));
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayProgressionSound()
    {
        progressionSound.Play();
    }

    public void PlayFailSound()
    {
        failSound.Play();
    }

    public void ChangeMusicVolume(int num)
    {
        PlayerPrefs.SetInt("Music_Volume", num);
        music.volume = num / 100.0f;
    }

    public void ChangeUiVolume(int num)
    {
        PlayerPrefs.SetInt("UI_Volume", num);
        clickSound.volume = num / 100.0f;
        progressionSound.volume = num / 100.0f;
    }
}
