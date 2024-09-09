using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityName : MonoBehaviour
{
    //Singleton
    public static CityName s;
    public static string name;

    void Awake() 
    {
        s = this; 
    }

    private void Update()
    {
        name = GetCityName();
    }

    public string GetCityName()
    {
        return gameObject.GetComponent<InputField>().text;
    }
}
