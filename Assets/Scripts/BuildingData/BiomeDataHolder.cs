using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeDataHolder : MonoBehaviour
{
    [Header("Efficiency per biome")]
    [Range(0f, 1f)]
    public float desert;
    [Range(0f, 1f)]
    public float forest;
    [Range(0f, 1f)]
    public float jungle;
    [Range(0f, 1f)]
    public float plains;
    [Range(0f, 1f)]
    public float savanna;
    [Range(0f, 1f)]
    public float shrubland;
    [Range(0f, 1f)]
    public float swamp;
    [Range(0f, 1f)]
    public float teiga;
    [Range(0f, 1f)]
    public float tundra;
}
