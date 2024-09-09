using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public ResidentialTooltip residentialTooltip;
    public IndustryTooltip industryTooltip;
    public CommercialTooltip commercialTooltip;
    public SpecialTooltip specialTooltip;

    private static Color badColor = new Color(1, 0.4489564f, 0.3803922f);
    private static Color goodColor = new Color(0.3820755f, 1, 0.4014933f);
    private static Color neutralColor = new Color(1,1,1);

    private static Color color;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        HideIndustryTooltip();
        HideResidentialTooltip();
        HideSpecialTooltip();
        HideCommercialTooltip();
    }

    public static void showIndustryTooltip(string header, Texture image, eBuilding building)
    {
        CostDataHolder costData;
        BuildingDataHolder buildingData;
        BiomeDataHolder biomeData;
        GetData(building, out costData, out buildingData, out biomeData);
        
        string costs = MakeCostsString(costData);
        string production = MakeProductionString(buildingData);
        string consumption = MakeConsumptionString(buildingData);
        string efficiency = MakeEfficiencyString(biomeData);
        string jobs = MakeJobsString(buildingData);
        string happiness = MakeHappinessString(buildingData, eTooltipType.Industry);
        current.industryTooltip.SetContents(header, costs, production, consumption, efficiency, jobs, happiness, image, color);
        current.industryTooltip.gameObject.SetActive(true);
    }

    public static void showResidentailTooltip(string header, Texture image, eBuilding building)
    {
        CostDataHolder costData;
        BuildingDataHolder buildingData;
        BiomeDataHolder biomeData;
        GetData(building, out costData, out buildingData, out biomeData);

        string costs = MakeCostsString(costData);
        string consumption = MakeConsumptionString(buildingData);
        string residents = MakeResidentsString(buildingData);
        string happiness = MakeHappinessString(buildingData, eTooltipType.Residential);
        current.residentialTooltip.SetContents(header, costs, consumption, residents, happiness, image);
        current.residentialTooltip.gameObject.SetActive(true);
    }

    public static void showCommercialTooltip(string header, Texture image, eBuilding building)
    {
        CostDataHolder costData;
        BuildingDataHolder buildingData;
        BiomeDataHolder biomeData;
        GetData(building, out costData, out buildingData, out biomeData);

        string costs = MakeCostsString(costData);
        string production = MakeProductionString(buildingData);
        string consumption = MakeConsumptionString(buildingData);
        current.commercialTooltip.SetContents(header, costs, production, consumption, image);
        current.commercialTooltip.gameObject.SetActive(true);
    }

    public static void showSpecialTooltip(string header, Texture image, eBuilding building)
    {
        CostDataHolder costData;
        BuildingDataHolder buildingData;
        BiomeDataHolder biomeData;
        GetData(building, out costData, out buildingData, out biomeData);

        string costs = MakeCostsString(costData);
        string production = MakeProductionString(buildingData);
        string consumption = MakeConsumptionString(buildingData);
        string effect = SetEffectString(building, buildingData);
        string jobs = MakeJobsString(buildingData);
        string happiness = MakeHappinessString(buildingData, eTooltipType.Special);
        current.specialTooltip.SetContents(header, costs, production, consumption, effect, jobs, happiness, image, color);
        current.specialTooltip.gameObject.SetActive(true);
    }

    public static void HideIndustryTooltip()
    {
        current.industryTooltip.gameObject.SetActive(false);
    }

    public static void HideResidentialTooltip()
    {
        current.residentialTooltip.gameObject.SetActive(false);
    }

    public static void HideCommercialTooltip()
    {
        current.commercialTooltip.gameObject.SetActive(false);
    }

    public static void HideSpecialTooltip()
    {
        current.specialTooltip.gameObject.SetActive(false);
    }

    private static string MakeResidentsString(BuildingDataHolder data)
    {
        return "Residents: " + data.maxPeople;
    }

    private static string SetEffectString(eBuilding building, BuildingDataHolder data)
    {
        switch (building)
        {
            case eBuilding.library:
            case eBuilding.school:
                return "produces culture";
            case eBuilding.church:
                return "produces culture and increases happiness of surrounding residential buildings within 10 tiles";
            case eBuilding.brewery:
                return "produces culture and increases happiness of surrounding residential buildings within 15 tiles";
            case eBuilding.castle:
                return "produces culture and significantly increases happiness of surrounding residential buildings within 30 tiles";
            case eBuilding.barracks:
                return "increases happiness of surrounding residential buildings witih 25 tiles";
            case eBuilding.mill:
                return "increases food production of every farm within " + data.maxMultDist
                    + " tiles (each farm can only be affeced by " + data.maxFoodMultiplier + " mills)";
            case eBuilding.dungeon:
                return "decreases happiness of surrounding residential buildings within 15 tiles, but increases happiness"
                    + " output of any barracks within 30 tiles (each barracks can only be affected by 1 dungeon)";
            default:
                return "ERROR";
        }
    }

    private static string MakeHappinessString(BuildingDataHolder data, eTooltipType type)
    {
        if (type == eTooltipType.Residential)
        {
            return "Happiness: " + data.baseHappiness;
        }
        else
        {
            if (data.happyProduction == 0)
            {
                color = neutralColor;
                return "No effect on happiness";
            }
            else
            {
                string str;
                if (data.happyProduction < 0)
                {
                    color = badColor;
                    str = "";
                }
                else
                {
                    color = goodColor;
                    str = "+";
                }
                str += data.happyProduction + " happiness within " + data.maxHappyDist + " tiles.";
                return str;
            }
        }
    }

    private static string MakeCostsString(CostDataHolder data)
    {
        string costs = data.money + " money\n";
        if (data.food != 0)
            costs += data.food + " food\n";
        if (data.wood != 0)
            costs += data.wood + " wood\n";
        if (data.stone != 0)
            costs += data.stone + " stone\n";
        if (data.iron != 0)
            costs += data.iron + " iron\n";
        if (data.goods != 0)
            costs += data.goods + "goods";
        
        return costs;
    }

    public static string MakeProductionString(BuildingDataHolder data)
    {
        string prod = "";
        if (data.moneyProduction != 0)
            prod += data.moneyProduction + " money\n";
        if (data.foodProduction != 0)
            prod += data.foodProduction + " food\n";
        if (data.woodProduction != 0)
            prod += data.woodProduction + " wood\n";
        if (data.stoneProduction != 0)
            prod += data.stoneProduction + " stone\n";
        if (data.rawIronProduction != 0)
            prod += data.rawIronProduction + " iron\n";
        if (data.goodsProduction != 0)
            prod += data.goodsProduction + " goods\n";
        if (data.culture != 0)
            prod += data.culture + " culture";

        return prod;
    }

    public static string MakeConsumptionString(BuildingDataHolder data)
    {
        string str = "";
        if (data.moneyConsumption != 0)
            str += data.moneyConsumption + " money\n";
        if (data.foodConsumption != 0)
            str += data.foodConsumption + " food\n";
        if (data.woodConsumption != 0)
            str += data.woodConsumption + " wood\n";
        if (data.stoneConsumption != 0)
            str += data.stoneConsumption + " stone\n";
        if (data.rawIronConsumption != 0)
            str += data.rawIronConsumption + " iron\n";
        if (data.goodsConsumption != 0)
            str += data.goodsConsumption + " goods";

        if (str == "")
            str = "nothing";

        return str;
    }

    public static string MakeEfficiencyString(BiomeDataHolder data)
    {
        int tundraVal = Mathf.RoundToInt(data.tundra * 100);
        int savannaVal = Mathf.RoundToInt(data.savanna * 100);
        int teigaVal = Mathf.RoundToInt(data.teiga * 100);
        int desertVal = Mathf.RoundToInt(data.desert * 100);
        int forestVal = Mathf.RoundToInt(data.forest * 100);
        int plainsVal = Mathf.RoundToInt(data.plains * 100);
        int swampVal = Mathf.RoundToInt(data.swamp * 100);
        int jungleVal = Mathf.RoundToInt(data.jungle * 100);
        int shrublandVal = Mathf.RoundToInt(data.shrubland * 100);

        string str = "tundra: " + tundraVal + "%     ";
        if (tundraVal < 100)
            str += "  ";
        if (tundraVal == 0)
            str += "  ";
        str += "savanna: " + savannaVal + "%\n";

        str += "teiga: " + teigaVal + "%        ";
        if (teigaVal < 100)
            str += "  ";
        if (teigaVal == 0)
            str += "  ";
        str += "desert: " + desertVal + "%\n";

        str += "forest: " + forestVal + "%       ";
        if (forestVal < 100)
            str += "  ";
        if (forestVal == 0)
            str += "  ";
        str += "plains: " + plainsVal + "%\n";

        str += "swamp: " + swampVal + "%    ";
        if (swampVal < 100)
            str += "  ";
        if (swampVal == 0)
            str += "  ";
        str += "jungle: " + jungleVal + "%\n";

        str += "shrubland: " + shrublandVal + "%";

        return str;
    }

    public static string MakeJobsString(BuildingDataHolder data)
    {
        return "Jobs: " + data.maxJobs;
    }

    public static void GetData(eBuilding building, out CostDataHolder costData, out BuildingDataHolder data, out BiomeDataHolder biomeData)
    {
        string location = "";
        switch(building)
        {
            case eBuilding.stoneMine:
                location = "stoneMine";
                break;
            case eBuilding.stoneWorker:
                location = "stoneWorker";
                break;
            case eBuilding.farm:
                location = "farm";
                break;
            case eBuilding.woodCutter:
                location = "woodCutter";
                break;
            case eBuilding.woodWorker:
                location = "woodWorker";
                break;
            case eBuilding.house:
                location = "house";
                break;
            case eBuilding.library:
                location = "library";
                break;
            case eBuilding.brewery:
                location = "brewery";
                break;
            case eBuilding.mill:
                location = "mill";
                break;
            case eBuilding.store:
                location = "store";
                break;
            case eBuilding.church:
                location = "church";
                break;
            case eBuilding.blacksmith:
                location = "blacksmith";
                break;
            case eBuilding.barracks:
                location = "barracks";
                break;
            case eBuilding.dungeon:
                location = "dungeon";
                break;
            case eBuilding.school:
                location = "school";
                break;
            case eBuilding.castle:
                location = "castle";
                break;
            case eBuilding.slums:
                location = "slums";
                break;
            case eBuilding.ironMine:
                location = "ironMine";
                break;
            default:
                Debug.LogError("unsupported building");
                break;
        }
        data = GameObject.Find("DataHolders/" + location).GetComponent<BuildingDataHolder>();
        costData = GameObject.Find("DataHolders/" + location).GetComponent<CostDataHolder>();
        biomeData = GameObject.Find("DataHolders/" + location).GetComponent<BiomeDataHolder>();
    }
}

public enum eTooltipType
{ 
    Residential,
    Commercial,
    Industry,
    Special
}
