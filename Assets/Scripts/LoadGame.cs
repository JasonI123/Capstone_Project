using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public Button City_1, City_2, City_3, City_4, City_5;
    public Button City_1X, City_2X, City_3X, City_4X, City_5X;
    public Text City_1_Text, City_2_Text, City_3_Text, City_4_Text, City_5_Text;
    public GameObject AreYouSureMenu;
    public GameObject LoadScreen;

    public static int deleteNum;

    void Update()
    {
        if (MenuManager.cityDeleted)
        {
            MenuManager.cityDeleted = false;
            LoadMenuSetup();
        }
    }

    private string GetCityName(int i)
    {
        return PlayerPrefs.GetString("City_" + i.ToString() + "_Name");
    }

    public void LoadMenuSetup()
    {
        City_1.interactable = false;
        City_2.interactable = false;
        City_3.interactable = false;
        City_4.interactable = false;
        City_5.interactable = false;
        City_1X.interactable = false;
        City_2X.interactable = false;
        City_3X.interactable = false;
        City_4X.interactable = false;
        City_5X.interactable = false;

        if (PlayerPrefs.GetInt("City_1") == 1)
        {
            City_1.interactable = true;
            City_1X.interactable = true;
            City_1_Text.text = GetCityName(1);
        }
        if (PlayerPrefs.GetInt("City_2") == 1)
        {
            City_2.interactable = true;
            City_2X.interactable = true;
            City_2_Text.text = GetCityName(2);
        }
        if (PlayerPrefs.GetInt("City_3") == 1)
        {
            City_3.interactable = true;
            City_3X.interactable = true;
            City_3_Text.text = GetCityName(3);
        }
        if (PlayerPrefs.GetInt("City_4") == 1)
        {
            City_4.interactable = true;
            City_4X.interactable = true;
            City_4_Text.text = GetCityName(4);
        }
        if (PlayerPrefs.GetInt("City_5") == 1)
        {
            City_5.interactable = true;
            City_5X.interactable = true;
            City_5_Text.text = GetCityName(5);
        }
    }

    public void Load(int i)
    {
        SaveGame.mainFile = "City_" + i.ToString();
        PlayerPrefs.SetInt("City_" + i.ToString(), 1);
        ResourceManager.city_num = i;
        SaveGame.newGame = false;
        Instantiate(LoadScreen);
        SceneManager.LoadScene(1);
    }

    public void X(int i)
    {
        MenuManager.cityToDelete = i;
        AreYouSureMenu.SetActive(true);
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }
}
