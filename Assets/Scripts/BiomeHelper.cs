using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEngine.WSA;

public class BiomeHelper : MonoBehaviour
{
    [Header("General Settings")]
    public Transform tiles;
    public TempDictionaries tempDictionaries;

    [Header("Tundra Setings")]
    public GameObject tundraBasePrefab;

    [Header("Teiga Settings")]
    public GameObject teigaBasePrefab;
    public GameObject teigaTreePrefab;
    public float teigaTreeRarity = 0.25f;

    [Header("Savanna Settings")]
    public GameObject savannaBasePrefab;
    public GameObject savannaBushPrefab;
    public GameObject savannaTreePrefab;
    public float savannaBushRarity;
    public float savannaTreeRarity;

    [Header("Shrubland Settings")]
    public GameObject shrublandBasePrefab;
    public GameObject shrublandBushPrefab;
    public float shrublandBushRarity = 0.65f;

    [Header("Forest Settings")]
    public GameObject forestBasePrefab;
    public GameObject forestTreePrefab;
    public GameObject forestBushPrefab;
    public float forestTreeRarity = 0.25f;
    public float forestBushRarity = 0.1f;

    [Header("Swamp Settings")]
    public GameObject swampBasePrefab;
    public GameObject swampMudPrefab;
    public GameObject swampTreePrefab;
    public GameObject swampTreeMudPrefab;
    public float swampMudRarity = 0.4f;
    public float swampTreeRarity = 0.15f;
    public float swampTreeMudRarity = 0.1f;

    [Header("Desert Settings")]
    public GameObject desertBasePrefab;
    public GameObject desertCactusPrefab;
    public float cactusRarity = 0.1f;

    [Header("Plains Settings")]
    public GameObject plainsBasePrefab;
    public GameObject plainsBushPrefab;
    public float plainsBushRarity = 0.15f;

    [Header("Jungle Settings")]
    public GameObject jungleBasePrefab;
    public GameObject jungleTreePrefab;
    public GameObject jungleBushPrefab;
    public float jungleTreeRarity = 0.80f;
    public float jungleBushRarity = 0.15f;

    [Header("Water Settings")]
    public GameObject waterBasePrefab;

    [Header("Save Game")]
    public SaveGame saveGame;

    [HideInInspector]
    public static eTerrainTile[][] tileMap;
    [HideInInspector]
    public static int[][] tileRotation;

    public void SetBiomeTerrainVariants()
    {
        tileMap = new eTerrainTile[100][];
        tileRotation = new int[100][];
        for (int i = 0; i < 100; i++)
        {
            tileMap[i] = new eTerrainTile[100];
            tileRotation[i] = new int[100];
        }


        for (int x = 0; x < 100; x++)
        {
            for(int z = 0; z < 100; z++)
            {
                GameObject tile = Map.mapGameobjs[x][z];
                // Set random rotation of variant
                int rotation = 90 * UnityEngine.Random.Range(0, 3);
                int rand = UnityEngine.Random.Range(0, 100);

                // Get biome of tile
                switch (Map.map[x][z])
                {
                    // Tundra
                    case eBiome.tundra:
                        placeTile(x, z, tundraBasePrefab, 0, eTerrainTile.tundraBase);
                        break;

                    // Teiga
                    case eBiome.teiga:
                        if (rand < teigaTreeRarity * 100)
                            placeTile(x, z, teigaTreePrefab, rotation, eTerrainTile.teigaTree);
                        else
                            placeTile(x, z, teigaBasePrefab, 0, eTerrainTile.teigaBase);
                        break;

                    // Savanna
                    case eBiome.savanna:
                        if (rand < savannaTreeRarity * 100)
                            placeTile(x, z, savannaTreePrefab, rotation, eTerrainTile.savannaTree);
                        else if ((rand -= (int)(savannaTreeRarity * 100)) < savannaBushRarity * 100)
                            placeTile(x, z, savannaBushPrefab, rotation, eTerrainTile.savannaBush);
                        else
                            placeTile(x, z, savannaBasePrefab, 0, eTerrainTile.savannaBase);
                        break;

                    // Shrubland
                    case eBiome.shrubland:
                        if (rand < shrublandBushRarity * 100)
                            placeTile(x, z, shrublandBushPrefab, rotation, eTerrainTile.shrublandBush);
                        else
                            placeTile(x, z, shrublandBasePrefab, 0, eTerrainTile.shrublandBase);
                        break;

                    // Forest
                    case eBiome.forest:
                        if (rand < forestTreeRarity * 100)
                            placeTile(x, z, forestTreePrefab, rotation, eTerrainTile.forestTree);
                        else if ((rand -= (int)(forestTreeRarity * 100)) < forestBushRarity * 100)
                            placeTile(x, z, forestBushPrefab, rotation, eTerrainTile.forestBush);
                        else
                            placeTile(x, z, forestBasePrefab, 0, eTerrainTile.forestBase);
                        break;

                    // Swamp
                    case eBiome.swamp:
                        if (rand < swampTreeRarity * 100)
                            placeTile(x, z, swampTreePrefab, rotation, eTerrainTile.swampTree);
                        else if ((rand -= (int)(swampTreeRarity * 100)) < swampTreeMudRarity * 100)
                            placeTile(x, z, swampTreeMudPrefab, rotation, eTerrainTile.swampTreeMud);
                        else if ((rand -= (int)(swampTreeMudRarity * 100)) < swampMudRarity * 100)
                            placeTile(x, z, swampMudPrefab, 0, eTerrainTile.swampMud);
                        else
                            placeTile(x, z, swampBasePrefab, 0, eTerrainTile.swampBase);
                        break;

                    // Desert
                    case eBiome.desert:
                        if (rand < cactusRarity * 100)
                            placeTile(x, z, desertCactusPrefab, rotation, eTerrainTile.desertCactus);
                        else
                            placeTile(x, z, desertBasePrefab, 0, eTerrainTile.desertBase);
                        break;

                    // Plains
                    case eBiome.plains:
                        if (rand < plainsBushRarity * 100)
                            placeTile(x, z, plainsBushPrefab, rotation, eTerrainTile.plainsBush);
                        else
                            placeTile(x, z, plainsBasePrefab, 0, eTerrainTile.plainsBase);
                        break;
                    // Jungle
                    case eBiome.jungle:
                        if (rand < jungleTreeRarity * 100)
                            placeTile(x, z, jungleTreePrefab, rotation, eTerrainTile.jungleTree);
                        else if ((rand -= (int)(jungleTreeRarity * 100)) < jungleBushRarity * 100)
                            placeTile(x, z, jungleBushPrefab, rotation, eTerrainTile.jungleBush);
                        else
                            placeTile(x, z, jungleBasePrefab, 0, eTerrainTile.jungleBase);
                        break;
                    // Water
                    case eBiome.water:
                        placeTile(x, z, waterBasePrefab, 0, eTerrainTile.waterBase);
                        break;
                    // Error
                    default:
                        Debug.Log("Error in world gen");
                        break;
                }
            }
        }

        saveGame.FirstSave();
    }

    public void placeTile(int x, int z, GameObject tileType, int rotation, eTerrainTile type)
    {
        GameObject temp = Map.mapGameobjs[x][z] = Instantiate(tileType);
        temp.name = "tile[" + x + "][" + z + "]";
        temp.transform.position = new Vector3(x, 0, z);
        temp.transform.parent = tiles;
        temp.transform.Rotate(new Vector3(0, rotation, 0));
        Destroy(temp.GetComponent<Collider>());
        tileMap[x][z] = type;
        tileRotation[x][x] = rotation;
    }
}

public enum eTerrainTile
{
    tundraBase,
    teigaBase,
    teigaTree,
    savannaBase,
    savannaBush,
    savannaTree,
    shrublandBase,
    shrublandBush,
    forestBase,
    forestTree,
    forestBush,
    swampBase,
    swampMud,
    swampTree,
    swampTreeMud,
    desertBase,
    desertCactus,
    plainsBase,
    plainsBush,
    jungleBase,
    jungleTree,
    jungleBush,
    waterBase
}
