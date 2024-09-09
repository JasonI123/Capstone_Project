using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationTime : MonoBehaviour
{
    public int num = 0;

    private void FixedUpdate()
    {
        if (num < 10)
            num++;
        else
            num = 0;

    }
}
