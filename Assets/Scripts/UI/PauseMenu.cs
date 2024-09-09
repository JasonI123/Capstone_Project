using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject LoadMenu;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        Instantiate(LoadMenu);
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
