using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progression : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private ResourceManager resourceManager;
    [SerializeField]
    private Button[] lockedLevel1;
    [SerializeField]
    private Button[] lockedLevel2;
    [SerializeField]
    private Button[] lockedLevel3;

    [SerializeField]
    private int popLevel1;
    [SerializeField]
    private int popLevel2;
    [SerializeField]
    private int popLevel3;

    [SerializeField]
    private int cultureLevel1;
    [SerializeField]
    private int cultureLevel2;
    [SerializeField]
    private int cultureLevel3;

    [SerializeField]
    private GameObject ProgressionCanvas;
    [SerializeField]
    private GameObject canvasLvl1;
    [SerializeField]
    private GameObject canvasLvl2;
    [SerializeField]
    private GameObject canvasLvl3;
    [SerializeField]
    private GameObject canvasUnlocked;

    [SerializeField]
    private Button canvasTab1;
    [SerializeField]
    private Button canvasTab2;
    [SerializeField]
    private Button canvasTab3;

    [SerializeField]
    private Text curPop;
    [SerializeField]
    private Text curCulture;
    [SerializeField]
    private Text reqPop;
    [SerializeField]
    private Text reqCulture;

    private SimulationTime timer;
    private ResourceManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = Camera.main.gameObject.GetComponent<ResourceManager>();
        timer = Camera.main.GetComponent<SimulationTime>();
        ProgressionCanvas.SetActive(false);
        
        foreach(Button b in lockedLevel1)
        {
            b.interactable = false;
        }
        foreach (Button b in lockedLevel2)
        {
            b.interactable = false;
        }
        foreach (Button b in lockedLevel3)
        {
            b.interactable = false;
        }

        if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") >= 1)
        {
            UnlockLevel(1);
        }
        if(PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") >= 2)
        {
            UnlockLevel(2);
        }
        if(PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") >= 3)
        {
            UnlockLevel(3);
        }
    }

    public void SetActiveTab(int tab)
    {
        canvasTab1.interactable = true;
        canvasTab2.interactable = true;
        canvasTab3.interactable = true;

        if (tab == 1)
            canvasTab1.interactable = false;
        else if (tab == 2)
            canvasTab2.interactable = false;
        else
            canvasTab3.interactable = false;
    }

    public void SetCanvasTab(int tab)
    {
        SetActiveTab(tab);
        if (tab <= PlayerPrefs.GetInt(SaveGame.mainFile + "_Level"))
        {
            Debug.Log(PlayerPrefs.GetInt(SaveGame.mainFile + "_Level"));
            canvasUnlocked.SetActive(true);
        }
        else
        {
            canvasUnlocked.SetActive(false);
        }
        if (tab == 1)
        {
            canvasLvl1.SetActive(true);
            canvasLvl2.SetActive(false);
            canvasLvl3.SetActive(false);
            reqPop.text = "Required Population: " + popLevel1.ToString();
            reqCulture.text = "Required Culture: " + cultureLevel1.ToString();
        }
        else if (tab == 2)
        {
            canvasLvl1.SetActive(false);
            canvasLvl2.SetActive(true);
            canvasLvl3.SetActive(false);
            reqPop.text = "Required Population: " + popLevel2.ToString();
            reqCulture.text = "Required Culture: " + cultureLevel2.ToString();
        }
        else if (tab == 3)
        {
            canvasLvl1.SetActive(false);
            canvasLvl2.SetActive(false);
            canvasLvl3.SetActive(true);
            reqPop.text = "Required Population: " + popLevel3.ToString();
            reqCulture.text = "Required Culture: " + cultureLevel3.ToString();
        }
    }

    public void OpenProgressionMenu(bool unlockedMilestone = false)
    {
        Time.timeScale = 0.0f;
        ProgressionCanvas.SetActive(true);

        int milestone = 1;

        curPop.text = "Current Population: " + resourceManager.population;
        curCulture.text = "Current Culture: " + resourceManager.culture;

        // Set the milestone to open
        if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 0)
        {
            milestone = 1;
        }
        else if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 1)
        {
            if (unlockedMilestone)
                milestone = 1;
            else 
                milestone = 2;
        }
        else if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 2)
        {
            if (unlockedMilestone)
                milestone = 2;
            else
                milestone = 3;
        }
        else if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 3)
        {
            milestone = 3;
        }

        // Set the correct milestone active
        SetCanvasTab(milestone);
    }

    public void CloseProgressionCanvas()
    {
        Time.timeScale = 1.0f;
        ProgressionCanvas.SetActive(false);
    }

    void UnlockLevel(int level)
    {
        if (level == 1)
        {
            foreach (Button b in lockedLevel1)
            {
                b.interactable = true;
            }
        }
        if (level == 2)
        {
            foreach (Button b in lockedLevel2)
            {
                b.interactable = true;
            }
        }
        if (level == 3)
        {
            foreach (Button b in lockedLevel3)
            {
                b.interactable = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer.num == 0 && PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") < 3)
        {
            if(PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 2)
            {
                if(manager.population >= popLevel3 && manager.culture >= cultureLevel3) 
                {
                    audioManager.PlayProgressionSound();
                    UnlockLevel(3);
                    PlayerPrefs.SetInt(SaveGame.mainFile + "_Level", 3);
                    OpenProgressionMenu(true);
                }
            }
            else if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 1)
            {
                if (manager.population >= popLevel2 && manager.culture >= cultureLevel2)
                {
                    audioManager.PlayProgressionSound();
                    UnlockLevel(2);
                    PlayerPrefs.SetInt(SaveGame.mainFile + "_Level", 2);
                    OpenProgressionMenu(true);
                }
            }
            else if (PlayerPrefs.GetInt(SaveGame.mainFile + "_Level") == 0)
            {
                if (manager.population >= popLevel1 && manager.culture >= cultureLevel1)
                {
                    audioManager.PlayProgressionSound();
                    UnlockLevel(1);
                    PlayerPrefs.SetInt(SaveGame.mainFile + "_Level", 1);
                    OpenProgressionMenu(true);
                }
            }
        }
    }
}
