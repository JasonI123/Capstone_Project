using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject LoadingScreen;

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Instantiate(LoadingScreen);
        SceneManager.LoadScene(0);
    }

    public void LastSave()
    {
        Instantiate(LoadingScreen);
        SceneManager.LoadScene(1);
    }
}
