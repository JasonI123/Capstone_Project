using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResidentialTooltip : MonoBehaviour
{
    public Text headerField;
    public Text costsField;
    public Text consumptionField;
    public Text residentsField;
    public Text happinessField;
    public RawImage imageField;

    public void SetContents(string header, string costs, string consumption, string residents, string happiness, Texture image)
    {
        headerField.text = header;
        costsField.text = costs;
        consumptionField.text = consumption;
        residentsField.text = residents;
        happinessField.text = happiness;
        imageField.texture = image;
    }
}
