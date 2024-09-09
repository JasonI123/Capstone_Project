using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static bool cityDeleted = false;
    public static int cityToDelete;

    public GameObject NewCityMenu1;
    public GameObject NewCityMenu2;
    public GameObject LoadMenu;
    public GameObject AreYouSureMenu;
    public GameObject CreditScreen;
    public GameObject Options;

    private void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            if (!PlayerPrefs.HasKey("City_" + i.ToString()))
            {
                PlayerPrefs.SetInt("City_" + i.ToString(), 0);
            }
            if (!PlayerPrefs.HasKey("City_" + i.ToString() + "_Name"))
            {
                PlayerPrefs.SetString("City_" + i.ToString() + "_Name", "Placeholder Name");
            }
            if (!PlayerPrefs.HasKey("City_" + i.ToString() + "_Level"))
            {
                PlayerPrefs.SetInt("City_" + i.ToString() + "_Level", 0);
            }
        }
    }

    public void Hide_CreditScreen()
    {
        CreditScreen.SetActive(false);
    }

    public void Show_CreditScreen()
    {
        CreditScreen.SetActive(true);
    }

    public void Show_NewCityMenu1()
    {
        NewCityMenu1.SetActive(true);
        NewCityMenu1.GetComponent<StartNewCity>().StartNewCityStartup();
    }

    public void Show_NewCityMenu2()
    {
        NewCityMenu2.SetActive(true);
    }

    public void Show_LoadMenu()
    {
        LoadMenu.SetActive(true);
        LoadMenu.GetComponent<LoadGame>().LoadMenuSetup();
    }

    public void Show_Options()
    {
        Options.SetActive(true);
    }

    public void AreYouSure_Yes()
    {
        PlayerPrefs.SetInt("City_" + cityToDelete + "_Level", 0);
        PlayerPrefs.SetInt("City_" + cityToDelete, 0);
        cityDeleted = true;
        AreYouSureMenu.SetActive(false);
    }

    public void AreYouSure_No()
    {
        AreYouSureMenu.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenWebsite(string url)
    {
        Application.OpenURL(url);
    }
}
