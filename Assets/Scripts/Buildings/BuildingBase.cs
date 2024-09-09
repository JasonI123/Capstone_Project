using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    protected ResourceManager manager;
    protected BuildingManager bldManager;
    protected SimulationTime timer;
    protected JobSource jobs;

    [SerializeField]
    private string dataLocation;

    [SerializeField]
    private bool isHouse = false;

    public BuildingDataHolder data;
    public BiomeDataHolder biomeData;
    public CostDataHolder cost;
    public eBuilding meBuilding;

    private bool start = true;
    public bool isFixedSubbuilding = false;

    public int x, z;

    public GameObject groundObj;
    public BiomeHelper biomeHelper;

    public eBiome biome;

    public static bool recentPlace = false;

    private void FixedUpdate()
    {
        if (timer.num == 10 && meBuilding != eBuilding.subbuilding)
        {
            if (bldManager.isFixed)
                OnSimulationUpdate();
        }

        if ((meBuilding == eBuilding.subbuilding && !isFixedSubbuilding) || (meBuilding != eBuilding.subbuilding && !bldManager.isFixed))
        {
            Destroy(groundObj);
            Vector3 pos = new Vector3(0, 0, 1000);
              Quaternion rot = Quaternion.identity;

            // Biome
            switch (Map.map[x][z])
            {
                case eBiome.tundra:
                    groundObj = Instantiate(biomeHelper.tundraBasePrefab, pos, rot);
                    break;
                case eBiome.teiga:
                    groundObj = Instantiate(biomeHelper.teigaBasePrefab, pos, rot);
                    break;
                case eBiome.savanna:
                    groundObj = Instantiate(biomeHelper.savannaBasePrefab, pos, rot);
                    break;
                case eBiome.shrubland:
                    groundObj = Instantiate(biomeHelper.shrublandBasePrefab, pos, rot);
                    break;
                case eBiome.forest:
                    groundObj = Instantiate(biomeHelper.forestBasePrefab, pos, rot);
                    break;
                case eBiome.swamp:
                    if (BiomeHelper.tileMap[x][z] == eTerrainTile.swampBase || BiomeHelper.tileMap[x][z] == eTerrainTile.swampTree)
                    {
                        groundObj = Instantiate(biomeHelper.swampBasePrefab, pos, rot);
                    }
                    else
                    {
                        groundObj = Instantiate(biomeHelper.swampMudPrefab, pos, rot);
                    }
                    break;
                case eBiome.desert:
                    groundObj = Instantiate(biomeHelper.desertBasePrefab, pos, rot);
                    break;
                case eBiome.plains:
                    groundObj = Instantiate(biomeHelper.plainsBasePrefab, pos, rot);
                    break;
                case eBiome.jungle:
                    groundObj = Instantiate(biomeHelper.jungleBasePrefab, pos, rot);
                    break;
                case eBiome.water:
                    groundObj = Instantiate(biomeHelper.waterBasePrefab, pos, rot);
                    break;
            }

            Destroy(groundObj.GetComponent<Collider>());
            groundObj.transform.position = new Vector3(x - 49.5f, 0.001f, z - 49.5f);
        }

        if (isFixedSubbuilding && start || meBuilding != eBuilding.subbuilding && bldManager.isFixed && start)
        {
            x = (int)(gameObject.transform.position.x + 49.5f);
            z = (int)(gameObject.transform.position.z + 49.5f);
            biome = Map.map[x][z];

            if (data.happyProduction != 0)
            {
                foreach (House h in manager.houseList)
                {
                    h.AddHappiness(this.transform.position, data.happyProduction, data.maxHappyDist);
                }
            }

            manager.culture += data.culture;

            // Cost
            manager.money -= cost.money;
            manager.totalWood -= cost.wood;
            manager.totalStone -= cost.stone;
            manager.totalFood -= cost.food;
            manager.totalStone -= cost.iron;
            manager.totalGoods -= cost.goods;

            start = false;
        }
        else if (start)
        {
            x = (int)(gameObject.transform.position.x + 49.5f);
            z = (int)(gameObject.transform.position.z + 49.5f);
            biome = Map.map[x][z];
        }
    }

    private void Start()
    {
        {
            data = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BuildingDataHolder>();
            biomeData = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BiomeDataHolder>();
            cost = GameObject.Find("DataHolders/" + dataLocation).GetComponent<CostDataHolder>();
            manager = Camera.main.gameObject.GetComponent<ResourceManager>();
            bldManager = gameObject.GetComponent<BuildingManager>();
            jobs = this.gameObject.GetComponent<JobSource>();
            timer = Camera.main.GetComponent<SimulationTime>();
            biomeHelper = GameObject.Find("NoiseGeneration").GetComponent<BiomeHelper>();
        }
    }

    public float GetBiomeMult()
    {
        switch (biome)
        {
            case eBiome.desert:
                return biomeData.desert;
            case eBiome.forest:
                return biomeData.forest;
            case eBiome.jungle:
                return biomeData.jungle;
            case eBiome.plains:
                return biomeData.plains;
            case eBiome.savanna: 
                return biomeData.savanna;
            case eBiome.shrubland:
                return biomeData.shrubland;
            case eBiome.swamp:
                return biomeData.swamp;
            case eBiome.teiga: 
                return biomeData.teiga;
            case eBiome.tundra:
                return biomeData.tundra;
            default:
                return 1;
        }
    }

    public void OnSimulationUpdate()
    {
        if (!isHouse && bldManager.placed)
        {
            // Money
            manager.money += data.moneyProduction;
            manager.money -= data.moneyConsumption;

            // Consumption
            int foodConsumed = (manager.totalFood > data.foodConsumption ? data.foodConsumption : manager.totalFood);
            int woodConsumed = (manager.totalWood > data.woodConsumption ? data.woodConsumption : manager.totalWood);
            int stoneConsumed = (manager.totalStone > data.stoneConsumption ? data.stoneConsumption : manager.totalStone);
            int ironConsumed = (manager.totalIron > data.rawIronConsumption ? data.rawIronConsumption : manager.totalIron);
            int goodsConsumed = (manager.totalGoods > data.goodsConsumption ? data.goodsConsumption : manager.totalGoods);

            manager.totalFood -= foodConsumed;
            manager.totalWood -= woodConsumed;
            manager.totalStone -= stoneConsumed;
            manager.totalIron -= ironConsumed;
            manager.totalGoods -= goodsConsumed;
            
            // Producion
            float productionMultiplier = Mathf.Min(
                data.maxJobs > 0 ? jobs.workers / ((float)(data.maxJobs)): 1,
                (data.foodConsumption > 0 ? (((float)foodConsumed) / data.foodConsumption) : 1),
                (data.woodConsumption > 0 ? (((float)woodConsumed) / data.woodConsumption) : 1),
                (data.stoneConsumption > 0 ? (((float)stoneConsumed) / data.stoneConsumption) : 1),
                (data.rawIronConsumption > 0 ? (((float)ironConsumed) / data.rawIronProduction) : 1),
                (data.goodsConsumption > 0 ? (((float)goodsConsumed) / data.goodsConsumption) : 1));

            productionMultiplier *= GetBiomeMult();
            productionMultiplier *= manager.GetHappinessMultiplier();

            manager.totalWood += Mathf.RoundToInt(productionMultiplier * data.woodProduction);
            manager.totalStone += Mathf.RoundToInt(productionMultiplier * data.stoneProduction);
            manager.totalIron += Mathf.RoundToInt(productionMultiplier * data.rawIronProduction);
            manager.totalGoods += Mathf.RoundToInt(productionMultiplier * data.goodsProduction);
        }
    }

    private void OnDestroy()
    {
        if (recentPlace == false)
        {
            if (meBuilding == eBuilding.subbuilding)
            {
                Map.mapBldgs[x][z] = eBuilding.none;
		return;
            }
	
	    else if (data.happyProduction == 0)
	    {
		return;
	    }

            foreach (House h in manager.houseList)
            {
                h.AddHappiness(this.transform.position, -data.happyProduction, data.maxHappyDist);
            }
        }
        else
            recentPlace = false;
    }
}
