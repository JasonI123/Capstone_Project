using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraignVisibilityManager : MonoBehaviour
{
    public void UpdateTerraignVisibility()
    {
        for (int x = 0; x < 100; x++)
        {
            for (int z = 0; z < 100; z++)
            {
                if (Map.mapBldgs[x][z] != eBuilding.none)
                    Map.mapGameobjs[x][z].SetActive(false);
                else
                    Map.mapGameobjs[x][z].SetActive(true);
            }
        }
    }
}
