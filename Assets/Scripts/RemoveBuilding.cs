using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class RemoveBuilding : MonoBehaviour
{
    private Ray _ray;
    private RaycastHit _hit;
    public bool click1 = false;
    public bool click2 = false;
    private Vector3 init;
    private Vector3 cur;
    private List<GameObject> destroyerList = new List<GameObject>();

    public bool destroyMode = false;
    public LayerMask groundLayerMask;
    public GameObject destroyer;
    public static List<BuildingManager> destroyList = new List<BuildingManager>();
    public RoadPlacement roadPlacement;
    public bool stopDestroy = false;
    public TerraignVisibilityManager terraignVisibilityManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (destroyMode && !click1 && !click2)
            {
                click1 = true;
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, groundLayerMask))
                {
                    init = ClampToNearest(_hit.point);
                }
            }
            else if (destroyMode && click1)
            {
                click1 = false;
                click2 = true;
            }
        }

        if (Input.GetMouseButtonDown(1) || stopDestroy)
        {
            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                destroyList[i].destroy = false;
                destroyList.RemoveAt(i);
            }

            for (int i = destroyerList.Count - 1; i >= 0; i--)
            {
                Destroy(destroyerList[i]);
                destroyerList.RemoveAt(i);
            }

            stopDestroy = false;
            destroyMode = false;
            click1 = false;
            click2 = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (destroyMode)
        {
            Road[] roads = roadPlacement.roadParent.GetComponentsInChildren<Road>();
            foreach (Road road in roads)
            {
                road.OnRoadsPlaced();
            }
        }

        if (click1 && destroyMode && !stopDestroy)
        {
            foreach (BuildingManager b in destroyList)
            {
                b.setDestroy(false);
            }

            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                destroyList[i].destroy = false;
                destroyList.RemoveAt(i);
            }

            for (int i = destroyerList.Count - 1; i >= 0 ; i--)
            {
                Destroy(destroyerList[i]);
                destroyerList.RemoveAt(i);
            }

            // Current mouse position
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, groundLayerMask))
            {
                cur = ClampToNearest(_hit.point);
            }

            // Convert to map's x and z coordinates for easier processing
            Vector3Int mapInit = VectorToMapCoords(init);
            Vector3Int mapCur = VectorToMapCoords(cur);

            if (mapInit == mapCur)
            {
                destroyerList.Add(Instantiate(destroyer));
                destroyerList[0].transform.position = MapCoordsToVector(mapInit);
            }
            else
            {
                int minX = Math.Min(mapInit.x, mapCur.x);
                int maxX = Math.Max(mapInit.x, mapCur.x);
                int minZ = Math.Min(mapInit.z, mapCur.z);
                int maxZ = Math.Max(mapInit.z, mapCur.z);

                int i = 0;
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        destroyerList.Add(Instantiate(destroyer));
                        destroyerList[i].transform.position = MapCoordsToVector(x, z);
                        i++;
                    }
                }
            }
        }
        else if (click2 && destroyMode && !stopDestroy)
        {
            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                Vector3Int coords = VectorToMapCoords(destroyList[i].transform.position);
                Map.mapBldgs[coords.x][coords.z] = eBuilding.none;
                Destroy(destroyList[i].gameObject);
                destroyList.RemoveAt(i);
            }
            for (int i = destroyerList.Count - 1; i >= 0; i--)
            {
                Destroy(destroyerList[i]);
                destroyerList.RemoveAt(i);
            }

            terraignVisibilityManager.UpdateTerraignVisibility();
            click2 = false;
        }
        else if (destroyMode && !stopDestroy)
        {
            for (int i = destroyList.Count - 1; i >= 0; i--)
            {
                destroyList[i].destroy = false;
                destroyList.RemoveAt(i);
            }

            for (int i = destroyerList.Count - 1; i >= 0; i--)
            {
                Destroy(destroyerList[i]);
                destroyerList.RemoveAt(i);
            }

            // Current mouse position
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, groundLayerMask))
            {
                cur = ClampToNearest(_hit.point);
            }

            destroyerList.Add(Instantiate(destroyer));
            destroyerList[0].transform.position = cur;    
        }
    }

    private Vector3 MapCoordsToVector(Vector3Int coords)
    {
        float x = (float)coords.x - 49.5f;
        float z = (float)coords.z - 49.5f;
        return new Vector3(x, 0, z);
    }

    private Vector3 MapCoordsToVector(int x, int z)
    {
        Vector3Int coords = new Vector3Int(x, 0, z);
        return MapCoordsToVector(coords);
    }

    private Vector3Int VectorToMapCoords(Vector3 vec)
    {
        int x = Mathf.RoundToInt(vec.x + 49.5f);
        int z = Mathf.RoundToInt(vec.z + 49.5f);
        return new Vector3Int(x, 0, z);
    }

    private Vector3 ClampToNearest(Vector3 pos)
    {
        Vector3 v = ((Vector3)Vector3Int.FloorToInt(pos));

        // (recenter in middle of cells)
        float s = 0.5f;
        v.x += s;
        v.z += s;

        return v;
    }
}
