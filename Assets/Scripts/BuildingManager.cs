using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    private string dataLocation;

    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public MeshRenderer[] meshComponents;
    private Dictionary<MeshRenderer, List<Material>> initialMaterials;
    public bool placed = false;
    public float efficiency;
    public float price;

    [HideInInspector] public bool hasValidPlacement;
    [HideInInspector] public bool isFixed;
    [HideInInspector] public bool costWithinLimits;

    public CostDataHolder cost;
    public BiomeDataHolder biomeData;
    private ResourceManager manager;

    public int _nObstacles;
    private int _nRoadPlacers;
    private bool _isRoad;
    private bool _badBiome = true;
    private Road _roadScript;

    public BuildingBase[] subBuildings;
    public bool destroy = false;

    private void Awake()
    {
        if (this.TryGetComponent<Road>(out _roadScript))
            _isRoad = true;
        else
            _isRoad = false;
        hasValidPlacement = true;
        isFixed = true;
        costWithinLimits = true;
        _nObstacles = 0;
        _nRoadPlacers = 0;

        _InitializeMaterials();
        manager = Camera.main.GetComponent<ResourceManager>();
        biomeData = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BiomeDataHolder>();
        cost = GameObject.Find("DataHolders/" + dataLocation).GetComponent<CostDataHolder>();
    }

    public void setDestroy(bool b)
    {
        if(b)
        {
            SetPlacementMode(PlacementMode.Invalid);
        }
        else
        {
            SetPlacementMode(PlacementMode.Fixed);
        }
    }

    private void FixedUpdate()
    {
        if (isFixed) return;

        int x = (int)(gameObject.transform.position.x + 49.5f);
        int y = (int)(gameObject.transform.position.z + 49.5f);
        eBiome biome = Map.map[x][y];
        BuildingGridPlacer.bBiome = biome;

        switch (biome)
        {
            case eBiome.desert:
                if (biomeData.desert <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.forest:
                if (biomeData.forest <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                    print("forest");
                }
                else
                    _badBiome = false;
                break;
            case eBiome.jungle:
                if (biomeData.jungle <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.plains:
                if (biomeData.plains <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.savanna:
                if (biomeData.savanna <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.shrubland:
                if (biomeData.shrubland <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.swamp:
                if (biomeData.swamp <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.teiga:
                if (biomeData.teiga <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.tundra:
                if (biomeData.tundra <= 0.05f)
                {
                    SetPlacementMode(PlacementMode.Invalid);
                    hasValidPlacement = false;
                    _badBiome = true;
                }
                else
                    _badBiome = false;
                break;
            case eBiome.water:
                SetPlacementMode(PlacementMode.Invalid);
                hasValidPlacement = false;
                _badBiome = true;
                break;

        }

        if (_badBiome)
        {
            BuildingGridPlacer.SetBuildError(BuildError.invalidLocation, true);
            BuildingGridPlacer.validBiome = false;
        }
        else
        {
            BuildingGridPlacer.SetBuildError(BuildError.invalidLocation, false);
            BuildingGridPlacer.validBiome = true;
        }

        if (_nRoadPlacers == 0)
        {
            BuildingGridPlacer.SetBuildError(BuildError.notOnRoad, true);
            SetPlacementMode(PlacementMode.Invalid);
        }

        // Building cost
        if (!isFixed && hasValidPlacement)
        {
            bool money = false;
            bool notMoney = false;

            if (cost.money > manager.money)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                money = true;
            }
            if (cost.wood > manager.totalWood)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }
            if (cost.food > manager.totalFood)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }
            if (cost.stone > manager.totalStone)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }
            if (cost.food > manager.totalFood)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }
            if (cost.iron > manager.totalIron)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }
            if (cost.goods > manager.totalGoods)
            {
                SetPlacementMode(PlacementMode.Invalid);
                costWithinLimits = false;
                notMoney = true;
            }

            if (money)
                BuildingGridPlacer.SetBuildError(BuildError.cantAfford, true);
            else
                BuildingGridPlacer.SetBuildError(BuildError.cantAfford, false);

            if (notMoney)
                BuildingGridPlacer.SetBuildError(BuildError.notEnoughMaterials, true);
            else
                BuildingGridPlacer.SetBuildError(BuildError.notEnoughMaterials, false);
        }
        else if (_nObstacles == 0 && hasValidPlacement == false && !_badBiome && costWithinLimits && _nRoadPlacers > 0)
        {
            SetPlacementMode(PlacementMode.Valid);
            costWithinLimits = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "destroy")
        {
            destroy = true;
            RemoveBuilding.destroyList.Add(this);
        }

        if (isFixed) return;

        // ignore ground objects
        if (_IsGround(other.gameObject)) return;

        Debug.Log("test");
        // If the object is a road placer, increment _nRoadPlacers and return.
        if (_IsRoadPlacer(other.gameObject))
        {
            _nRoadPlacers++;
            BuildingGridPlacer.SetBuildError(BuildError.notOnRoad, false);
            return;
        }

        _nObstacles++;
        BuildingGridPlacer.SetBuildError(BuildError.overlapingItems, true);
        SetPlacementMode(PlacementMode.Invalid);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isFixed) return;

        // ignore ground objects
        if (_IsGround(other.gameObject)) return;

        // If the object is a road placer, decrement _nRoadPlacers, else decrement _nObstacles.
        if (_IsRoadPlacer(other.gameObject))
            _nRoadPlacers--;
        else
            _nObstacles--;

        if (_nObstacles == 0)
        {
            BuildingGridPlacer.SetBuildError(BuildError.overlapingItems, false);
        }

        if (_nRoadPlacers == 0)
        {
            BuildingGridPlacer.SetBuildError(BuildError.notOnRoad, true);
            SetPlacementMode(PlacementMode.Invalid);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _InitializeMaterials();
    }
#endif

    public void SetPlacementMode(PlacementMode mode)
    {
        if (mode == PlacementMode.Fixed)
        {
            isFixed = true;
            hasValidPlacement = true;
        }
        else if (mode == PlacementMode.Valid)
        {
            hasValidPlacement = true;
        }
        else
        {
            hasValidPlacement = false;
        }
        SetMaterial(mode);
    }

    public void SetMaterial(PlacementMode mode)
    {
        if (mode == PlacementMode.Fixed)
        {
            foreach (MeshRenderer r in meshComponents)
                r.sharedMaterials = initialMaterials[r].ToArray();
        }
        else
        {
            Material matToApply = mode == PlacementMode.Valid
                ? validPlacementMaterial : invalidPlacementMaterial;

            Material[] m; int nMaterials;
            foreach (MeshRenderer r in meshComponents)
            {
                nMaterials = initialMaterials[r].Count;
                m = new Material[nMaterials];
                for (int i = 0; i < nMaterials; i++)
                    m[i] = matToApply;
                r.sharedMaterials = m;
            }
        }
    }

    private void _InitializeMaterials()
    {
        if (initialMaterials == null)
            initialMaterials = new Dictionary<MeshRenderer, List<Material>>();
        if (initialMaterials.Count > 0)
        {
            foreach (var l in initialMaterials) l.Value.Clear();
            initialMaterials.Clear();
        }

        foreach (MeshRenderer r in meshComponents)
        {
            initialMaterials[r] = new List<Material>(r.sharedMaterials);
        }
    }

    private bool _IsRoadPlacer(GameObject o)
    {
        return ((1 << o.layer) & BuildingGridPlacer.instance.roadLayerMask.value) != 0;
    }

    private bool _IsGround(GameObject o)
    {
        return ((1 << o.layer) & BuildingGridPlacer.instance.groundLayerMask.value) != 0;
    }
}

public enum PlacementMode
{
    Fixed,
    Valid,
    Invalid
}