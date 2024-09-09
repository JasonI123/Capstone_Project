using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDestroyer : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Time.timeScale < 0.1f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void DestroyThis()
    {
        this.gameObject.SetActive(false);
    }
}
