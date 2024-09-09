using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject roadPlacerPrefab;
    public GameObject roadPlacer;
    private Transform parent;
    public Material Straight;
    public Material Curve;
    public Material T;
    public Material Crossing;

    public MeshRenderer[] meshComponents;
    private Dictionary<MeshRenderer, List<Material>> initialMaterials;
    public bool roadToNorth, roadToEast, roadToSouth, roadToWest;
    bool updateRoads = false;

    void Start()
    {
        parent = GameObject.Find("Roads").transform;
        roadPlacer = Instantiate(roadPlacerPrefab);
        _InitializeMaterials();
    }
    

    void FixedUpdate()
    {
        roadPlacer.transform.position = this.transform.position;
        roadPlacer.SetActive(true);
        if (updateRoads)
        {
            updateRoads = false;
            OnUpdateRoads();
        }
    }

    private void OnDestroy()
    {
        Destroy(roadPlacer);
    }

    public void OnRoadsPlaced()
    {
        updateRoads = true;
    }

    public void OnUpdateRoads()
    {
        Transform[] roads = parent.GetComponentsInChildren<Transform>();
        int numBorderingRoads = 0;
        roadToNorth = roadToEast = roadToSouth = roadToWest = false;
        int thisPosX = Mathf.FloorToInt(this.transform.position.x);
        int thisPosZ = Mathf.FloorToInt(this.transform.position.z);
        foreach (Transform road in roads)
        {
            int roadPosX = Mathf.FloorToInt(road.position.x);
            int roadPosZ = Mathf.FloorToInt(road.position.z);
            if (roadPosX == thisPosX + 1 && roadPosZ == thisPosZ)
            {
                roadToNorth = true;
                numBorderingRoads++;
            }
            else if (roadPosX == thisPosX - 1 && roadPosZ == thisPosZ)
            {
                roadToSouth = true;
                numBorderingRoads++;
            }
            else if (roadPosZ == thisPosZ + 1 && roadPosX == thisPosX)
            {
                roadToEast = true;
                numBorderingRoads++;
            }
            else if (roadPosZ == thisPosZ - 1 && roadPosX == thisPosX)
            {
                roadToWest = true;
                numBorderingRoads++;
            }

            if (numBorderingRoads == 0)
                SetMaterialCrossing();
            else if (numBorderingRoads == 1)
                SetMaterialStraight();
            else if (numBorderingRoads == 2)
            {
                if (roadToNorth && roadToSouth)
                    SetMaterialStraight();
                else if (roadToEast && roadToWest)
                    SetMaterialStraight();
                else
                    SetMaterialCurve();
            }
            else if (numBorderingRoads == 3)
                SetMaterialT();
            else //if (numBorderingRoads == 4)
                SetMaterialCrossing();
        }
    }

    private void SetMaterialStraight()
    {
        SetMaterial(Straight);
        this.transform.rotation = Quaternion.identity;

        if (roadToNorth || roadToSouth)
            this.transform.Rotate(new Vector3(0, 90, 0));
    }

    private void SetMaterialCurve()
    {
        SetMaterial(Curve);
        this.transform.rotation = Quaternion.identity;

        if (roadToNorth && roadToEast)
            this.transform.Rotate(new Vector3(0, 90, 0));
        else if (roadToEast && roadToSouth)
            this.transform.Rotate(new Vector3(0, 0, 0));
        else if (roadToSouth && roadToWest)
            this.transform.Rotate(new Vector3(0, -90, 0));
        else //if (roadToWest && roadToNorth)
            this.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void SetMaterialT()
    {
        SetMaterial(T);
        this.transform.rotation = Quaternion.identity;

        if (roadToNorth && roadToSouth && roadToEast)
            this.transform.Rotate(new Vector3(0, 90, 0));
        else if (roadToNorth && roadToSouth && roadToWest)
            this.transform.Rotate(new Vector3(0, -90, 0));
        else if (roadToEast && roadToWest && roadToNorth)
            this.transform.Rotate(new Vector3(0, 180, 0));
        else //if (roadToEast && roadToWest && roadToSouth)
            this.transform.Rotate(new Vector3(0, 0, 0));
    }

    private void SetMaterialCrossing()
    {
        SetMaterial(Crossing);
    }

    private void SetMaterial(Material matToApply)
    {
        Material[] m; int nMaterials;
        foreach (MeshRenderer r in meshComponents)
        {
            nMaterials = initialMaterials[r].Count;
            m = new Material[nMaterials];
            for (int i = 0; i < nMaterials; i++)
                m[i] = matToApply;
            r.sharedMaterials = m;
        }
    }

    private void _InitializeMaterials()
    {
        if (initialMaterials == null)
            initialMaterials = new Dictionary<MeshRenderer, List<Material>>();
        if (initialMaterials.Count > 0)
        {
            foreach (var l in initialMaterials) l.Value.Clear();
            initialMaterials.Clear();
        }

        foreach (MeshRenderer r in meshComponents)
        {
            initialMaterials[r] = new List<Material>(r.sharedMaterials);
        }
    }
}

