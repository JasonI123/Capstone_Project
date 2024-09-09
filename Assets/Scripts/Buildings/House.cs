using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class House : MonoBehaviour
{
    [SerializeField]
    private string dataLocation;

    public int people;
    public int happiness;
    public ResourceManager manager;
    protected BuildingManager bldManager;
    public SimulationTime timer;
    protected JobSource jobs;
    private bool start = true;

    public BuildingDataHolder data;

    private void FixedUpdate()
    {
        if (timer.num == 10)
        {
            if (bldManager.isFixed)
                OnSimulationUpdate();
        }
        if (bldManager.isFixed && start)
        {
            manager.houseList.Add(this);
            if (data.happyProduction != 0)
            {
                foreach (House h in manager.houseList)
                {
                    h.AddHappiness(this.transform.position, data.happyProduction, data.maxHappyDist);
                }
            }
            start = false;
        }
    }

    private void Start()
    {
        data = GameObject.Find("DataHolders/" + dataLocation).GetComponent<BuildingDataHolder>();
        manager = Camera.main.gameObject.GetComponent<ResourceManager>();
        bldManager = gameObject.GetComponent<BuildingManager>();
        jobs = this.gameObject.GetComponent<JobSource>();
        timer = Camera.main.GetComponent<SimulationTime>();
    }

    protected void OnSimulationUpdate()
    {
        manager.population -= people;
        if (manager.totalFood < data.foodConsumption)
        {
            people = manager.totalFood;
            manager.totalFood = 0;
        }
        else
        {
            people = data.maxPeople;
            manager.totalFood -= people;
        }
        manager.population += people;
        manager.money += people;
        if (people != 0)
            manager.totalWood -= Mathf.Min(manager.totalWood, data.woodConsumption);
    }
    public void AddHappiness(Vector3 other, int happy, int maxHappyDist)
    {
        Vector3 cur = this.transform.position;

        int distanceX = (int) Mathf.Abs(other.x - cur.x);
        int distanceZ = (int) Mathf.Abs(other.z - cur.z);

        if (distanceZ <= maxHappyDist && distanceX < maxHappyDist)
        {
            happiness += happy;
        }
    }

    private void OnDestroy()
    {
        manager.population -= people;
        manager.houseList.Remove(this);
    }
}
