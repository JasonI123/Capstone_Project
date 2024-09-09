using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static int city_num;

    public int money = 1000;
    public int totalFood = 100;
    public int totalWood = 100;
    public int totalStone = 10;
    public int totalIron = 5;
    public int totalGoods = 10;
    public int population = 0;
    public int jobsFilled = 0;
    public int jobsAvailable = 0;
    public int avgHappiness = 0;
    public int culture = 0;
    public List<JobSource> unfilledJobsList;
    public List<GameObject> farmList;
    public List<House> houseList;
    public GameObject GameOverScreen;
    public Text GameOverReason;

    [Header("Stats in UI")]
    public Text moneyText;
    public Text foodText;
    public Text woodText;
    public Text stoneText;
    public Text ironText;
    public Text goodsText;
    public Text populationText;
    public Text happinessText;
    public Text unemplymentText;
    public Text emptyJobsText;
    public InputField cityNameText;

    public SimulationTime timer;

    public bool redoJobs = false;

    public Help help;

    public AudioManager audioManager;

    private bool simTimeOverride = false;

    public void OpenHelp()
    {
        help.gameObject.SetActive(true);
        help.Setup();
    }

    public float GetHappinessMultiplier()
    {
        if (avgHappiness <= -3)
            return 0.7f;
        else if (avgHappiness == -2)
            return 0.8f;
        else if (avgHappiness == -1)
            return 0.9f;
        else if (avgHappiness == 0)
            return 1f;
        else if (avgHappiness == 1)
            return 1.1f;
        else if (avgHappiness == 2)
            return 1.2f;
        else //if (avgHappiness >= 3)
            return 1.3f;
    }

    public void NameChanged(bool confirmed)
    {
        if (confirmed)
        {
            PlayerPrefs.SetString("City_" + city_num + "_Name", cityNameText.text);
        }
        else
        {
            cityNameText.text = PlayerPrefs.GetString("City_" + city_num + "_Name");
        }
    }

    private void Start()
    {
        cityNameText.text = PlayerPrefs.GetString("City_" + city_num + "_Name");
        unfilledJobsList = new List<JobSource>();
        farmList = new List<GameObject>();
        houseList = new List<House>();
        timer = this.gameObject.GetComponent<SimulationTime>();
    }

    private void FixedUpdate()
    {
        moneyText.text = "Money: " + money;
        foodText.text = "Food: " + totalFood;

        if (totalFood <= 0)
        {
            GameOverScreen.SetActive(true);
            audioManager.PlayFailSound();
            Time.timeScale = 0;
            GameOverReason.text = "You ran out of food";
            return;
        }
        else if (money <= 0)
        {
            GameOverScreen.SetActive(true);
            audioManager.PlayFailSound();
            Time.timeScale = 0;
            GameOverReason.text = "You ran out of money";
            return;
        }

        if (jobsFilled > population || jobsFilled > jobsAvailable)
        {
            jobsAvailable = 0;
            jobsFilled = 0;

            while (unfilledJobsList.Count > 0)
            {
                unfilledJobsList.RemoveAt(0);
            }

            redoJobs = true;

            if (timer.num == 0)
                simTimeOverride = true;

            return;
        }
        else
        {
            redoJobs = false;
        }
        Debug.Log("This is a test");
        Debug.Log(jobsFilled);
        if (timer.num == 0 || simTimeOverride)
        {
            OnSimulationUpdate();
            simTimeOverride = false;
        }

        woodText.text = "Wood: " + totalWood;
        stoneText.text = "Stone: " + totalStone;
        ironText.text = "Iron: " + totalIron;
        goodsText.text = "Goods: " + totalGoods;
        populationText.text = "Population: " + population;
        emptyJobsText.text = "Empty Jobs: " + (jobsAvailable - jobsFilled);

        int unemployment = population - jobsFilled;
        unemplymentText.text = "Unemployment: " + unemployment;
        while (jobsFilled < population && jobsFilled < jobsAvailable)
        {
            int job = Random.Range(0, unfilledJobsList.Count - 1);
            JobSource j = unfilledJobsList[job];
            j.workers += 1;
            jobsFilled++;

            if (j.workers == j.data.maxJobs)
            {
                j.inJobList = false;
                unfilledJobsList.RemoveAt(job);
            }
        }
    }
    private void OnSimulationUpdate()
    {
        if (houseList.Count == 0)
        {
            avgHappiness = 0;
        }
        else
        {
            int temp = 0;
            foreach (House h in houseList)
            {
                if (h.happiness > 3)
                    temp += 3;
                if (h.happiness < -3)
                    temp -= 3;
                else
                    temp += h.happiness;
            }

            avgHappiness = Mathf.RoundToInt((float)temp / (float)houseList.Count);
        }
        happinessText.text = "Average Happiness: " + avgHappiness;
    }
}

public enum eResources
{
    money,
    food,
    wood,
    stone,
    iron,
    goods,
    none
}
