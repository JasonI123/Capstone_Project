using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    [SerializeField]
    private string dataLocation;

    public float multiplier = 1;
    protected ResourceManager manager;
    protected BuildingManager bldManager;
    protected SimulationTime timer;
    protected JobSource jobs;
    private BuildingBase bBase;

    public BuildingDataHolder data;

    private void FixedUpdate()
    {
        if (timer.num == 10)
        {
            if (bldManager.isFixed)
                OnSimulationUpdate();
        }
    }

    private void Start()
    {
        manager = Camera.main.gameObject.GetComponent<ResourceManager>();
        bldManager = gameObject.GetComponent<BuildingManager>();
        manager.farmList.Add(this.gameObject);
        jobs = this.gameObject.GetComponent<JobSource>();
        timer = Camera.main.GetComponent<SimulationTime>();
        data = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BuildingDataHolder>();
        bBase = this.gameObject.GetComponent<BuildingBase>();
    }

    protected void OnSimulationUpdate()
    {
        manager.totalFood += Mathf.FloorToInt((jobs.workers / (float)data.maxJobs) * data.foodProduction * GetMultipliers());    
    }

    private void OnDestroy()
    {
        manager.farmList.Remove(this.gameObject);
    }

    private float GetMultipliers()
    {
        float temp = bBase.GetBiomeMult();
        temp *= manager.GetHappinessMultiplier();
        temp *= multiplier;
        return temp;
    }
}
