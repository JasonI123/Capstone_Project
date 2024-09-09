using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static eBuilding[][] mapBldgs = new eBuilding[100][];
    public static eBiome[][] map = new eBiome[100][];
    public static GameObject[][] mapGameobjs = new GameObject[100][];
    public static float[][] mapBldRot = new float[100][];

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            map[i] = new eBiome[100];
            mapGameobjs[i] = new GameObject[100];
            mapBldgs[i] = new eBuilding[100];
            mapBldRot[i] = new float[100];
            for (int j = 0; j < 100; j++)
            {
                mapBldgs[i][j] = eBuilding.none;
                mapBldRot[i][j] = 0;
            }
        }
    }
}

public enum eBuilding
{
    none,
    placeholder,
    subbuilding,
    house,
    road,
    brewery,
    farm,
    library,
    mill,
    stoneMine,
    stoneWorker,
    woodCutter,
    woodWorker,
    store,
    church,
    barracks,
    dungeon,
    school,
    castle,
    slums,
    ironMine,
    blacksmith
}


public enum eBiome
{
    tundra,
    teiga,
    savanna,
    shrubland,
    forest,
    swamp,
    desert,
    plains,
    jungle,
    water
}


