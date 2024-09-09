using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSource : MonoBehaviour
{
    private ResourceManager manager;
    private BuildingManager bldManager;
    private bool jobsAdded = false;

    public string dataLocation;
    public bool inJobList = false;
    public int workers;
    public BuildingDataHolder data;
    public static bool recentPlace = false;

    private void Start()
    {
        data = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BuildingDataHolder>();
        manager = Camera.main.gameObject.GetComponent<ResourceManager>();
        bldManager = gameObject.GetComponent<BuildingManager>();
    }
    private void FixedUpdate()
    {
        if (bldManager.placed == false)
            return;

        if (manager.redoJobs)
        {
            inJobList = false;
            workers = 0;
        }

        if (workers < data.maxJobs && !inJobList)
        {
            inJobList = true;
            manager.unfilledJobsList.Add(this);
            manager.jobsAvailable += data.maxJobs - workers;
        }
    }

    private void OnDestroy()
    {
        Road r;
        if (!this.gameObject.TryGetComponent<Road>(out r) && recentPlace == false)
        {
            manager.jobsAvailable -= data.maxJobs;
            manager.jobsFilled -= workers;
        }
        else
            recentPlace = false;
    }
}
