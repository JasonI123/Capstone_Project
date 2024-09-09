using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    private int cur = 0;

    public Text header;
    public GameObject[] images;
    public Button next;
    public Button prev;
    public void Setup()
    {
        cur = 0;

        images[0].SetActive(true);
        for (int i = 1; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }

        prev.interactable = false;
        next.interactable = true;
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }

    private void SetImage()
    {
        header.text = "Help (page " + (cur + 1).ToString() + ")";

        for (int i = 0; i < images.Length; i++)
        {
            if (i == cur)
                images[i].SetActive(true);
            else
                images[i].SetActive(false);
        }
    }

    public void Next()
    {
        cur++;
        if (cur >= images.Length)
        {
            cur = images.Length - 1;
            prev.interactable = true;
            next.interactable = false;
            return;
        }

        prev.interactable = true;
        if (cur == images.Length - 1)
        {
            next.interactable = false;
        }

        SetImage();
    }

    public void Prev()
    {
        cur--;
        if (cur < 0)
        {
            cur = 0;
            prev.interactable = false;
            next.interactable = true;
            return;
        }

        next.interactable = true;
        if (cur == 0)
        {
            prev.interactable = false;
        }

        SetImage();
    }
}
