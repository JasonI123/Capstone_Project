using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MakeNoiseMap : MonoBehaviour
{
    public Transform tiles;
    [Header("Noise settings")]
    // The more octaves, the longer generation will take
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public float noiseScale;
    public int mapWidth;
    public int mapHeight;
    public Vector2 offset;
    [Range(0, 1)]
    public float waterHeight;
    public float waterNoiseScale;
    [SerializeField]
    private NoiseValue[] TileTypes;
    public BiomeHelper helper;
    public SaveGame saveGame;

    private void Start()
    {
        if (SaveGame.newGame == true)
        {
            PlayerPrefs.SetInt(SaveGame.mainFile + "_Level", 0);
            PlayerPrefs.SetInt(SaveGame.mainFile + "_Happiness", 0);
            ApplyNoise();
        }
        else
        {
            saveGame.Load();
            tiles.transform.position = new Vector3(-0.5f * mapWidth + 0.5f, 0, -0.5f * mapHeight + 0.5f);
        }
    }

    void ApplyNoise()
    {
        // Generate random seed
        int seed1 = UnityEngine.Random.Range(0, int.MaxValue);
        int seed2 = UnityEngine.Random.Range(0, int.MaxValue);

        // Pass along our parameters to generate our noise
        var noiseMapHeat = WorldGenNoise.GenerateNoiseMap(mapWidth, mapHeight, seed1, noiseScale, octaves, persistance, lacunarity, offset);
        var noiseMapHumid = WorldGenNoise.GenerateNoiseMap(mapWidth, mapHeight, seed2, noiseScale, octaves, persistance, lacunarity, offset);
        var noiseMapLakes = WorldGenNoise.GenerateNoiseMap(mapWidth, mapHeight, seed2, waterNoiseScale, octaves, persistance, lacunarity, offset);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // If this tile is water, set it to water and
                // move on to the next loop.
                var water = noiseMapLakes[y * mapWidth + x];
                if (water <= waterHeight)
                {
                    Map.map[x][y] = eBiome.water;
                    continue;
                }


                // Get temperature and humidity at this position
                var heat = noiseMapHeat[y * mapWidth + x];
                var humidity = noiseMapHumid[y * mapWidth + x];


                // Loop over our configured tile types
                for (int i = 0; i < TileTypes.Length - 1; i++)
                {
                    // Select the correct tile for the heat and humidity
                    bool brk = false;
                    if (heat <= TileTypes[i].Heat)
                    {
                        if (humidity <= TileTypes[i].Humidity)
                        {
                            Map.map[x][y] = (eBiome)i;
                            brk = true;
                        }
                        if (brk) break;
                    }
                }
            }
        }
        helper.SetBiomeTerrainVariants();
        tiles.transform.position = new Vector3(-0.5f * mapWidth + 0.5f, 0, -0.5f * mapHeight + 0.5f);
    }
}

[Serializable]
class NoiseValue
{
    [Range(0f, 1f)]
    public float Heat;
    [Range(0f, 1f)]
    public float Humidity;
    public GameObject GroundTile;
}