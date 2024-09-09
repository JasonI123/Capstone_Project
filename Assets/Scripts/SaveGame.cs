using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveGame : MonoBehaviour
{
    public static string mainFile;
    public static bool newGame = true;

    public TerraignVisibilityManager terraignVisibilityManager;
    public BiomeHelper biomeHelper;
    public ResourceManager resourceManager;

    public GameObject brewery;
    public GameObject church;
    public GameObject farm;
    public GameObject house;
    public GameObject library;
    public GameObject mill;
    public GameObject road;
    public GameObject stoneMine;
    public GameObject stoneWorker;
    public GameObject store;
    public GameObject woodCutter;
    public GameObject woodWorker;
    public GameObject barracks;
    public GameObject dungeon;
    public GameObject school;
    public GameObject castle;
    public GameObject slums;
    public GameObject ironMine;
    public GameObject blacksmith;

    public void FirstSave()
    {
        StreamWriter writer1 = new StreamWriter(Application.persistentDataPath + "/" + mainFile + "_Biomes.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                writer1.Write((int)Map.map[i][j]);
                writer1.Write(" ");
            }
        }
        writer1.Close();

        StreamWriter writer2 = new StreamWriter(Application.persistentDataPath + "/" + mainFile + "_Tiles.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                writer2.Write((int)BiomeHelper.tileMap[i][j]);
                writer2.Write(" ");
            }
        }
        writer2.Close();

        Save();

        StreamWriter writer3 = new StreamWriter(Application.persistentDataPath + "/" + mainFile + "_TileRotation.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                writer3.Write((int)BiomeHelper.tileRotation[i][j]);
                writer3.Write(" ");
            }
        }
        writer3.Close();

        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(mainFile + "_Happiness", resourceManager.avgHappiness);

        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + mainFile + "_Buildings.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                writer.Write((int)Map.mapBldgs[i][j]);
                writer.Write(" ");
                writer.Write(Mathf.RoundToInt(Map.mapBldRot[i][j]));
                writer.Write(" ");
            }
        }
        writer.Close();

        StreamWriter writer2 = new StreamWriter(Application.persistentDataPath + "/" + mainFile + "_Resources.txt");
        writer2.Write(resourceManager.money);
        writer2.Write(" ");
        writer2.Write(resourceManager.totalFood);
        writer2.Write(" ");
        writer2.Write(resourceManager.totalWood);
        writer2.Write(" ");
        writer2.Write(resourceManager.totalStone);
        writer2.Write(" ");
        writer2.Write(resourceManager.totalIron);
        writer2.Write(" ");
        writer2.Write(resourceManager.totalGoods);
        writer2.Write(" ");
        writer2.Close();
    }

    public void Load()
    {
        resourceManager.avgHappiness = PlayerPrefs.GetInt(mainFile + "_Happiness");

        BiomeHelper.tileMap = new eTerrainTile[100][];
        BiomeHelper.tileRotation = new int[100][];
        for (int i = 0; i < 100; i++)
        {
            BiomeHelper.tileMap[i] = new eTerrainTile[100];
            BiomeHelper.tileRotation[i] = new int[100];
        }



        StreamReader reader1 = new StreamReader(Application.persistentDataPath + "/" + mainFile + "_Biomes.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                Map.map[i][j] = (eBiome)ReadInt(ref reader1);
            }
        }
        reader1.Close();
        StreamReader reader2 = new StreamReader(Application.persistentDataPath + "/" + mainFile + "_Tiles.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                BiomeHelper.tileMap[i][j] = (eTerrainTile)ReadInt(ref reader2);
            }
        }
        reader2.Close();

        int[][] rotations = new int[100][];
        StreamReader reader3 = new StreamReader(Application.persistentDataPath + "/" + mainFile + "_Buildings.txt");
        for (int i = 0; i < 100; i++)
        {
            rotations[i] = new int[100];
            for (int j = 0; j < 100; j++)
            {
                Map.mapBldgs[i][j] = (eBuilding)ReadInt(ref reader3);
                rotations[i][j] = ReadInt(ref reader3);
            }
        }
        reader3.Close();

        StreamReader reader4 = new StreamReader(Application.persistentDataPath + "/" + mainFile + "_TileRotation.txt");
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                BiomeHelper.tileRotation[i][j] = ReadInt(ref reader4);
            }
        }
        reader4.Close();

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0;j < 100; j++)
            {
                eTerrainTile tile = BiomeHelper.tileMap[i][j];
                int rot = BiomeHelper.tileRotation[i][j];
                switch (tile)
                {
                    case eTerrainTile.tundraBase:
                        biomeHelper.placeTile(i, j, biomeHelper.tundraBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.teigaBase:
                        biomeHelper.placeTile(i, j, biomeHelper.teigaBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.teigaTree:
                        biomeHelper.placeTile(i, j, biomeHelper.teigaTreePrefab, rot, tile);
                        break;
                    case eTerrainTile.savannaBase:
                        biomeHelper.placeTile(i, j, biomeHelper.savannaBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.savannaBush:
                        biomeHelper.placeTile(i, j, biomeHelper.savannaBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.savannaTree:
                        biomeHelper.placeTile(i, j, biomeHelper.savannaBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.shrublandBase:
                        biomeHelper.placeTile(i, j, biomeHelper.shrublandBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.shrublandBush:
                        biomeHelper.placeTile(i, j, biomeHelper.shrublandBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.forestBase:
                        biomeHelper.placeTile(i, j, biomeHelper.forestBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.forestTree:
                        biomeHelper.placeTile(i, j, biomeHelper.forestTreePrefab, rot, tile);
                        break;
                    case eTerrainTile.forestBush:
                        biomeHelper.placeTile(i, j, biomeHelper.forestBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.swampBase:
                        biomeHelper.placeTile(i, j, biomeHelper.swampBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.swampMud:
                        biomeHelper.placeTile(i, j, biomeHelper.swampMudPrefab, rot, tile);
                        break;
                    case eTerrainTile.swampTree:
                        biomeHelper.placeTile(i, j, biomeHelper.swampTreePrefab, rot, tile);
                        break;
                    case eTerrainTile.swampTreeMud:
                        biomeHelper.placeTile(i, j, biomeHelper.swampTreeMudPrefab, rot, tile);
                        break;
                    case eTerrainTile.desertBase:
                        biomeHelper.placeTile(i, j, biomeHelper.desertBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.desertCactus:
                        biomeHelper.placeTile(i, j, biomeHelper.desertCactusPrefab, rot, tile);
                        break;
                    case eTerrainTile.plainsBase:
                        biomeHelper.placeTile(i, j, biomeHelper.plainsBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.plainsBush:
                        biomeHelper.placeTile(i, j, biomeHelper.plainsBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.jungleBase:
                        biomeHelper.placeTile(i, j, biomeHelper.jungleBasePrefab, rot, tile);
                        break;
                    case eTerrainTile.jungleTree:
                        biomeHelper.placeTile(i, j, biomeHelper.jungleTreePrefab, rot, tile);
                        break;
                    case eTerrainTile.jungleBush:
                        biomeHelper.placeTile(i, j, biomeHelper.jungleBushPrefab, rot, tile);
                        break;
                    case eTerrainTile.waterBase:
                        biomeHelper.placeTile(i, j, biomeHelper.waterBasePrefab, rot, tile);
                        break;
                }

                if (Map.mapBldgs[i][j] == eBuilding.none || Map.mapBldgs[i][j] == eBuilding.subbuilding)
                    continue;

                float x = i - 49.5f;
                float z = j - 49.5f;
                GameObject building;

                switch (Map.mapBldgs[i][j])
                {
                    case eBuilding.house:
                        building = Instantiate(house);
                        break;
                    case eBuilding.road:
                        building = Instantiate(road);
                        break;
                    case eBuilding.brewery:
                        building = Instantiate(brewery);
                        break;
                    case eBuilding.farm:
                        building = Instantiate(farm);
                        break;
                    case eBuilding.library:
                        building = Instantiate(library);
                        break;
                    case eBuilding.mill:
                        building = Instantiate(mill);
                        break;
                    case eBuilding.stoneMine:
                        building = Instantiate(stoneMine);
                        break;
                    case eBuilding.stoneWorker:
                        building = Instantiate(stoneWorker);
                        break;
                    case eBuilding.woodCutter:
                        building = Instantiate(woodCutter);
                        break;
                    case eBuilding.woodWorker:
                        building = Instantiate(woodWorker);
                        break;
                    case eBuilding.store:
                        building = Instantiate(store);
                        break;
                    case eBuilding.church:
                        building = Instantiate(church);
                        break;
                    case eBuilding.barracks:
                        building = Instantiate(barracks);
                        break;
                    case eBuilding.dungeon:
                        building = Instantiate(dungeon);
                        break;
                    case eBuilding.school:
                        building = Instantiate(school);
                        break;
                    case eBuilding.castle:
                        building = Instantiate(castle);
                        break;
                    case eBuilding.slums:
                        building = Instantiate(slums);
                        break;
                    case eBuilding.ironMine:
                        building = Instantiate(ironMine);
                        break;
                    case eBuilding.blacksmith:
                        building = Instantiate(blacksmith);
                        break;
                    default:
                        Debug.Log("Unsupported Building Type: " + Map.mapBldgs[i][j]);
                        building = Instantiate(house);
                        break;
                }

                building.transform.position = new Vector3(x, 0.02f, z);
                building.transform.rotation = Quaternion.identity;
                building.transform.Rotate(Vector3.up, rotations[i][j]);

                BuildingManager m = building.GetComponent<BuildingManager>();

                m.SetPlacementMode(PlacementMode.Fixed);
                m.placed = true;

                Road r;
                if (building.TryGetComponent<Road>(out r))
                {
                    Map.mapBldgs[i][j] = eBuilding.road;
                    building.transform.parent = GameObject.Find("Roads").transform;
                }
                else
                    Map.mapBldgs[i][j] = building.GetComponent<BuildingBase>().meBuilding;

                foreach (BuildingBase b in m.subBuildings)
                {
                    Map.mapBldgs[b.x][b.z] = eBuilding.subbuilding;
                    b.isFixedSubbuilding = true;
                }
            }
        }

        //Read Resources
        StreamReader reader5 = new StreamReader(Application.persistentDataPath + "/" + mainFile + "_Resources.txt");
        resourceManager.money = ReadInt(ref reader5);
        resourceManager.totalFood = ReadInt(ref reader5);
        resourceManager.totalWood = ReadInt(ref reader5);
        resourceManager.totalStone = ReadInt(ref reader5);
        resourceManager.totalIron = ReadInt(ref reader5);
        resourceManager.totalGoods = ReadInt(ref reader5);
        reader5.Close();

        terraignVisibilityManager.UpdateTerraignVisibility();

        Road[] roads = GameObject.Find("Roads").GetComponentsInChildren<Road>();
        foreach (Road road in roads)
        {
            road.OnRoadsPlaced();
        }
    }

    private int ReadInt(ref StreamReader reader)
    {
        char c;
        string s = "";

        c = (char)reader.Read();
        do
        {
            s += c;
            c = (char)reader.Read();
        }
        while (c != ' ');

        return int.Parse(s);
    }
}
