using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDictionaries : MonoBehaviour
{
    public Dictionary<eTerrainTile, GameObject> tileTypeToPrefab;
    public Dictionary<eTerrainTile, eBiome> tileTypeToBiome;

    [Header("Tile Type Prefabs")]
    public GameObject tundraBase;
    public GameObject teigaBase, teigaTree;
    public GameObject savannaBase, savannaBush, savannaTree;
    public GameObject shrublandBase, shrublandBush;
    public GameObject forestBase, forestTree, forestBush;
    public GameObject swampBase, swampMud, swampTree, swampTreeMud;
    public GameObject desertBase, desertCactus;
    public GameObject plainsBase, plainsBush;
    public GameObject jungleBase, jungleTree, jungleBush;
    public GameObject waterBase;

    void Start()
    {
        //tileTypeToPrefab
        tileTypeToPrefab = new Dictionary<eTerrainTile, GameObject>();
        tileTypeToPrefab.Add(eTerrainTile.tundraBase, tundraBase);
        tileTypeToPrefab.Add(eTerrainTile.teigaBase, teigaBase);
        tileTypeToPrefab.Add(eTerrainTile.teigaTree, teigaTree);
        tileTypeToPrefab.Add(eTerrainTile.savannaBase, savannaBase);
        tileTypeToPrefab.Add(eTerrainTile.savannaBush, savannaBush);
        tileTypeToPrefab.Add(eTerrainTile.savannaTree, savannaTree);
        tileTypeToPrefab.Add(eTerrainTile.shrublandBase, shrublandBase);
        tileTypeToPrefab.Add(eTerrainTile.shrublandBush, shrublandBush);
        tileTypeToPrefab.Add(eTerrainTile.forestBase, forestBase);
        tileTypeToPrefab.Add(eTerrainTile.forestTree, forestTree);
        tileTypeToPrefab.Add(eTerrainTile.forestBush, forestBush);
        tileTypeToPrefab.Add(eTerrainTile.swampBase, swampBase);
        tileTypeToPrefab.Add(eTerrainTile.swampMud, swampMud);
        tileTypeToPrefab.Add(eTerrainTile.swampTree, swampTree);
        tileTypeToPrefab.Add(eTerrainTile.swampTreeMud, swampTreeMud);
        tileTypeToPrefab.Add(eTerrainTile.desertBase, desertBase);
        tileTypeToPrefab.Add(eTerrainTile.desertCactus, desertCactus);
        tileTypeToPrefab.Add(eTerrainTile.plainsBase, plainsBase);
        tileTypeToPrefab.Add(eTerrainTile.plainsBush, plainsBush);
        tileTypeToPrefab.Add(eTerrainTile.jungleBase, jungleBase);
        tileTypeToPrefab.Add(eTerrainTile.jungleTree, jungleTree);
        tileTypeToPrefab.Add(eTerrainTile.jungleBush, jungleBush);
        tileTypeToPrefab.Add(eTerrainTile.waterBase, waterBase);

        //tileTypeToBiome
        tileTypeToBiome = new Dictionary<eTerrainTile, eBiome>();
        tileTypeToBiome.Add(eTerrainTile.tundraBase, eBiome.tundra);
        tileTypeToBiome.Add(eTerrainTile.teigaBase, eBiome.teiga);
        tileTypeToBiome.Add(eTerrainTile.teigaTree, eBiome.teiga);
        tileTypeToBiome.Add(eTerrainTile.savannaBase, eBiome.savanna);
        tileTypeToBiome.Add(eTerrainTile.savannaBush, eBiome.savanna);
        tileTypeToBiome.Add(eTerrainTile.savannaTree, eBiome.savanna);
        tileTypeToBiome.Add(eTerrainTile.shrublandBase, eBiome.shrubland);
        tileTypeToBiome.Add(eTerrainTile.shrublandBush, eBiome.shrubland);
        tileTypeToBiome.Add(eTerrainTile.forestBase, eBiome.forest);
        tileTypeToBiome.Add(eTerrainTile.forestTree, eBiome.forest);
        tileTypeToBiome.Add(eTerrainTile.forestBush, eBiome.forest);
        tileTypeToBiome.Add(eTerrainTile.swampBase, eBiome.swamp);
        tileTypeToBiome.Add(eTerrainTile.swampMud, eBiome.swamp);
        tileTypeToBiome.Add(eTerrainTile.swampTree, eBiome.swamp);
        tileTypeToBiome.Add(eTerrainTile.swampTreeMud, eBiome.swamp);
        tileTypeToBiome.Add(eTerrainTile.desertBase, eBiome.desert);
        tileTypeToBiome.Add(eTerrainTile.desertCactus, eBiome.desert);
        tileTypeToBiome.Add(eTerrainTile.plainsBase, eBiome.plains);
        tileTypeToBiome.Add(eTerrainTile.plainsBush, eBiome.plains);
        tileTypeToBiome.Add(eTerrainTile.jungleBase, eBiome.jungle);
        tileTypeToBiome.Add(eTerrainTile.jungleTree, eBiome.jungle);
        tileTypeToBiome.Add(eTerrainTile.jungleBush, eBiome.jungle);
        tileTypeToBiome.Add(eTerrainTile.waterBase, eBiome.water);
    }
}
