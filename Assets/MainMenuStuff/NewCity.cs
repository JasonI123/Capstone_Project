using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewCity : MonoBehaviour
{
    public Button button;
    public GameObject LoadScreen;

    public static string cityName = "";

    private void Update()
    {
        Debug.Log(cityName);
        cityName = CityName.name;
        if(cityName == "")
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void OnCreateCityButton()
    {
        SaveGame.mainFile = "City_" + StartNewCity.cityButtonClicked.ToString();
        PlayerPrefs.SetInt("City_" + StartNewCity.cityButtonClicked.ToString(), 1);
        PlayerPrefs.SetString("City_" + StartNewCity.cityButtonClicked.ToString() + "_Name", cityName);
        ResourceManager.city_num = StartNewCity.cityButtonClicked;
        SaveGame.newGame = true;
        Instantiate(LoadScreen);
        SceneManager.LoadScene(1);
    }

    public void OnCancelButton()
    {
        CityName.s.gameObject.GetComponentInParent<Canvas>().gameObject.SetActive(false);
    }
}
