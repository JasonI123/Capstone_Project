using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class BuildButtonsHandler : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject ResidentialButtonPopup;
    public GameObject CommercialButtonPopup;
    public GameObject IndustryButtonPopup;
    public GameObject SpecialButtonPopup;
    public GameObject MenuButtonPopup;
    public RemoveBuilding removeBuilding;
    public Text BuildErrors;
    public Text BuildBiome;

    private Color badBiomeColor = new Color(0.6980392f, 0, 0);
    private Color goodBiomeColor = new Color(0.1960784f, 0.1960784f, 0.1960784f);
    public void DeleteButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(false);
        removeBuilding.destroyMode = true;
    }

    public void RoadButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(false);
        removeBuilding.stopDestroy = true;
    }

    public void ResidentialButtonClicked()
    {
        ResidentialButtonPopup.SetActive(true);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(false);
        removeBuilding.stopDestroy = true; ;
    }

    public void CommercialButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(true);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(false);
        removeBuilding.stopDestroy = true;
    }

    public void IndustryButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(true);
        SpecialButtonPopup.SetActive(false);
        removeBuilding.stopDestroy = true;
    }

    public void SpecialButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(true);
        removeBuilding.stopDestroy = true;
    }

    public void MenuButtonClicked()
    {
        ResidentialButtonPopup.SetActive(false);
        CommercialButtonPopup.SetActive(false);
        IndustryButtonPopup.SetActive(false);
        SpecialButtonPopup.SetActive(false);
        MenuButtonPopup.SetActive(true);
        removeBuilding.stopDestroy = true;
        Time.timeScale = 0f;
    }

    public void ClearBuildBiomeText()
    {
        BuildBiome.text = "";
    }

    public void SetBuildBiomeText(eBiome biome, bool valid)
    {
        BuildBiome.text = "Biome:\n";
        switch (biome)
        {
            case eBiome.desert:
                BuildBiome.text += "desert";
                break;
            case eBiome.forest:
                BuildBiome.text += "forest";
                break;
            case eBiome.jungle:
                BuildBiome.text += "jungle";
                break;
            case eBiome.plains:
                BuildBiome.text += "plains";
                break;
            case eBiome.savanna:
                BuildBiome.text += "savanna";
                break;
            case eBiome.shrubland:
                BuildBiome.text += "shrubland";
                break;
            case eBiome.swamp:
                BuildBiome.text += "swamp";
                break;
            case eBiome.teiga:
                BuildBiome.text += "teiga";
                break;
            case eBiome.tundra:
                BuildBiome.text += "tundra";
                break;
            case eBiome.water:
                BuildBiome.text += "water";
                break;
        }

        if (valid)
        {
            BuildBiome.color = goodBiomeColor;
        }
        else
        {
            BuildBiome.color = badBiomeColor;
        }
    }

    public void SetErrorTextOutOfBounds()
    {
        BuildErrors.text = "Out of bounds";
    }

    public void SetBuildErrorText(bool clear = false)
    {
        if (clear)
        {
            BuildErrors.text = "";
        }
        else
        {
            bool addNewline = false;
            BuildErrors.text = "";
            if (BuildingGridPlacer.GetBuildError(BuildError.invalidLocation)
                || BuildingGridPlacer.GetBuildError(BuildError.notOnRoad))
            {
                BuildErrors.text += "Invalid Location";
                addNewline = true;
            }
            if (BuildingGridPlacer.GetBuildError(BuildError.overlapingItems))
            {
                if (addNewline)
                    BuildErrors.text += "\n";
                BuildErrors.text += "Overlapping Items";
                addNewline = true;
            }
            if (BuildingGridPlacer.GetBuildError(BuildError.cantAfford))
            {
                if (addNewline)
                    BuildErrors.text += "\n";
                BuildErrors.text += "Not Enough Money";
                addNewline = true;
            }
            if (BuildingGridPlacer.GetBuildError(BuildError.notEnoughMaterials))
            {
                if (addNewline)
                    BuildErrors.text += "\n";
                BuildErrors.text += "Not Enough Resources";
            }
        }
    }
}
