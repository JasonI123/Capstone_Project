using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CommercialTooltip : MonoBehaviour
{
    public Text headerField;
    public Text costsField;
    public Text productionField;
    public Text consumptionField;
    public RawImage imageField;

    public void SetContents(string header, string costs, string production, string consumption, Texture image)
    {
        headerField.text = header;
        costsField.text = costs;
        productionField.text = production;
        consumptionField.text = consumption;
        imageField.texture = image;
    }
}
