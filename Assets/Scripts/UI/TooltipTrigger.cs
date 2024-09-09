using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public Texture image;
    public eBuilding building;
    public eTooltipType tooltipType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (tooltipType)
        {
            case eTooltipType.Commercial:
                TooltipSystem.showCommercialTooltip(header, image, building);
                break;
            case eTooltipType.Industry:
                TooltipSystem.showIndustryTooltip(header, image, building);
                break;
            case eTooltipType.Residential:
                TooltipSystem.showResidentailTooltip(header, image, building);
                break;
            case eTooltipType.Special:
                TooltipSystem.showSpecialTooltip(header, image, building);
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (tooltipType)
        {
            case eTooltipType.Commercial:
                TooltipSystem.HideCommercialTooltip();
                break;
            case eTooltipType.Industry:
                TooltipSystem.HideIndustryTooltip();
                break;
            case eTooltipType.Residential:
                TooltipSystem.HideResidentialTooltip();
                break;
            case eTooltipType.Special:
                TooltipSystem.HideSpecialTooltip();
                break;
        }
    }
}
