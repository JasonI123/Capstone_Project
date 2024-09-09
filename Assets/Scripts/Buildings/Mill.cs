using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : MonoBehaviour
{
    [SerializeField]
    private string dataLocation;

    protected ResourceManager manager;
    protected JobSource jobs;
    protected SimulationTime timer;

    public BuildingDataHolder data;
    protected BuildingManager bldManager;

    private bool start = true;

    private void Start()
    {
        data = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BuildingDataHolder>();
        manager = Camera.main.gameObject.GetComponent<ResourceManager>();
        jobs = this.gameObject.GetComponent<JobSource>();
        bldManager = gameObject.GetComponent<BuildingManager>();
    }

    private void FixedUpdate()
    {
        if (bldManager.isFixed && start)
        {
            foreach (GameObject f in manager.farmList)
            {
                Vector3 other = f.transform.position;
                Vector3 cur = this.transform.position;
                if (other.x <= cur.x + data.maxMultDist && other.x >= cur.x - data.maxMultDist
                    && other.z <= cur.z + data.maxMultDist && other.z >= cur.z - data.maxMultDist
                    && f.GetComponent<Farm>().multiplier < data.maxFoodMultiplier)
                {
                    f.GetComponent<Farm>().multiplier += data.foodMultiplier;
                }
            }

            start = false;
            this.enabled = false;
        }
    }
}
