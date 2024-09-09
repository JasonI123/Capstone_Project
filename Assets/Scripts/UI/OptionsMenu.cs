using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider musicVolume;
    public Slider UIVolume;
    public AudioManager audioManager;

    private void Awake()
    {
        musicVolume.value = PlayerPrefs.GetInt("Music_Volume");
        UIVolume.value = PlayerPrefs.GetInt("UI_Volume");
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        audioManager.ChangeMusicVolume((int)musicVolume.value);
    }

    public void ChangeUiVolume()
    {
        audioManager.ChangeUiVolume((int)UIVolume.value);
    }
}
