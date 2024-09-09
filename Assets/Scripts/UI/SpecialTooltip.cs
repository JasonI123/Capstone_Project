using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpecialTooltip : MonoBehaviour
{
    public Text headerField;
    public Text costsField;
    public Text productionField;
    public Text consumptionField;
    public Text effectField;
    public Text jobsField;
    public Text happinessField;
    public RawImage imageField;

    public void SetContents(string header, string costs, string production, string consumption, string effect, string jobs, string happiness, Texture image, Color color)
    {
        headerField.text = header;
        costsField.text = costs;
        productionField.text = production;
        consumptionField.text = consumption;
        effectField.text = effect;
        jobsField.text = jobs;
        happinessField.text = happiness;
        happinessField.color = color;
        imageField.texture = image;
    }
}
