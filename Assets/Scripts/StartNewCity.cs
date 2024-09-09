using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartNewCity : MonoBehaviour
{
    public GameObject NewCityMenu2;
    public GameObject AreYouSureMenu;

    public Button Start_City_1;
    public Button Start_City_2;
    public Button Start_City_3;
    public Button Start_City_4;
    public Button Start_City_5; 
    
    public Button Start_City_1_x;
    public Button Start_City_2_x;
    public Button Start_City_3_x;
    public Button Start_City_4_x;
    public Button Start_City_5_x;

    public Text City_1_Text;
    public Text City_2_Text;
    public Text City_3_Text;
    public Text City_4_Text;
    public Text City_5_Text;

    public static int cityButtonClicked;

    void Update()
    {
        if (MenuManager.cityDeleted)
        {
            MenuManager.cityDeleted = false;
            StartNewCityStartup();
        }
    }

    public string GetCityName(int i)
    {
        return PlayerPrefs.GetString("City_" + i.ToString() + "_Name");
    }

    public void StartNewCityStartup()
    {
        Debug.Log(PlayerPrefs.GetInt("City_1"));
        // Set all buttons to be uninteractable (this will be undone for some).
        Start_City_1.interactable = false;
        Start_City_2.interactable = false;
        Start_City_3.interactable = false;
        Start_City_4.interactable = false;
        Start_City_5.interactable = false;
        Start_City_1_x.interactable = false;
        Start_City_2_x.interactable = false;
        Start_City_3_x.interactable = false;
        Start_City_4_x.interactable = false;
        Start_City_5_x.interactable = false;

        // Set Button Text From PlayerPrefs
        Debug.Log(PlayerPrefs.GetInt("City_1"));

        if (PlayerPrefs.GetInt("City_1") == 1)
            City_1_Text.text = GetCityName(1);
        else
            City_1_Text.text = "Empty";

        if (PlayerPrefs.GetInt("City_2") == 1)
            City_2_Text.text = GetCityName(2);
        else
            City_2_Text.text = "Empty";

        if (PlayerPrefs.GetInt("City_3") == 1)
            City_3_Text.text = GetCityName(3);
        else
            City_3_Text.text = "Empty";

        if (PlayerPrefs.GetInt("City_4") == 1)
            City_4_Text.text = GetCityName(4);
        else
            City_4_Text.text = "Empty";

        if (PlayerPrefs.GetInt("City_5") == 1)
            City_5_Text.text = GetCityName(5);
        else
            City_5_Text.text = "Empty";

        // Set Button Interactability From PlayerPrefs

        if (PlayerPrefs.GetInt("City_1") == 0)
            Start_City_1.interactable = true;
        else
            Start_City_1_x.interactable = true;

        if (PlayerPrefs.GetInt("City_2") == 0)
            Start_City_2.interactable = true;
        else
            Start_City_2_x.interactable = true;

        if (PlayerPrefs.GetInt("City_3") == 0)
            Start_City_3.interactable = true;
        else
            Start_City_3_x.interactable = true;

        if (PlayerPrefs.GetInt("City_4") == 0)
            Start_City_4.interactable = true;
        else
            Start_City_4_x.interactable = true;

        if (PlayerPrefs.GetInt("City_5") == 0)
            Start_City_5.interactable = true;
        else
            Start_City_5_x.interactable = true;
    }
    public void Start_City_num(int num)
    {
        cityButtonClicked = num;
        NewCityMenu2.SetActive(true);
    }

    public void Start_City_num_x(int num)
    {
        MenuManager.cityToDelete = num;
        AreYouSureMenu.SetActive(true);
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }
}
