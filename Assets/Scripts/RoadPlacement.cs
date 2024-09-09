using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadPlacement : MonoBehaviour
{
    public bool placeRoad = false;
    public GameObject roadPrefabObj;
    public BuildingGridPlacer placer;

    private bool _firstClick = true;
    private Vector3 _currentPos;
    private Ray _ray;
    private Camera _mainCamera;
    private RaycastHit _hit;
    private GameObject roadfirst;
    public List<GameObject> _tempRoads;
    public Transform roadParent;
    public TerraignVisibilityManager terraignVisibilityManager;
    private bool updateRoads = false;
    private bool validPlacement = false;

    private void Start()
    {
        _tempRoads = new List<GameObject>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateRoads)
        {
            updateRoads = false;
            Road[] roads = roadParent.GetComponentsInChildren<Road>();
            foreach (Road road in roads)
            {
                road.OnRoadsPlaced();
            }
        }
        if (placeRoad == false)
        {
            if (!_firstClick)
            {
                foreach (GameObject g in _tempRoads)
                    Destroy(g);
                _tempRoads.Clear();
            }

            _firstClick = true;
            if (roadfirst != null)
                Destroy(roadfirst);
        }

        if (placeRoad && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_firstClick)
                FirstClick();
            else
                SecondClick();
        }
    }

    private char ChooseDir(Vector3 _currentPos, Vector3 nextPos)
    {
        if (Mathf.Abs(_currentPos.x - nextPos.x) > Mathf.Abs(_currentPos.z - nextPos.z))
            return 'x';
        else
            return 'y';
    }

    private void FirstClick()
    {
        if (roadfirst == null)
            roadfirst = Instantiate(roadPrefabObj);

        // handle raycast for phantom road placement
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            _currentPos = _ClampToNearest(_hit.point, placer.cellSize);
            _currentPos.y += 0.01f;
            roadfirst.transform.position = _currentPos;

            int x = Mathf.RoundToInt(_currentPos.x + 49.5f);
            int z = Mathf.RoundToInt(_currentPos.z + 49.5f);
            BuildingManager m = roadfirst.GetComponent<BuildingManager>();

            if (Map.mapBldgs[x][z] == eBuilding.none)
            {
                validPlacement = true;
                m.SetPlacementMode(PlacementMode.Valid);
            }
            else
            {
                validPlacement = false;
                m.SetPlacementMode(PlacementMode.Invalid);
            }

            if (Input.GetMouseButtonDown(0))
            { // if left-click and the placement is valid.
                if (validPlacement)
                {
                    _firstClick = false;
                }
            }

            if (Map.mapBldgs[x][z] == eBuilding.none)
            {
                Map.mapBldgs[x][z] = eBuilding.placeholder;
                terraignVisibilityManager.UpdateTerraignVisibility();
                Map.mapBldgs[x][z] = eBuilding.none;
            }
            else
                terraignVisibilityManager.UpdateTerraignVisibility();
        }
        else
        {
            terraignVisibilityManager.UpdateTerraignVisibility();
        }
    }
    private void SecondClick()
    {
        if (roadfirst != null)
        {
            Destroy(roadfirst);
            roadfirst = null;
        }

        foreach (GameObject g in _tempRoads)
            Destroy(g);
        _tempRoads.Clear();

        Vector3 nextPos;
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            nextPos = _ClampToNearest(_hit.point, placer.cellSize);
            char dir = ChooseDir(_currentPos, nextPos);
            // Choose x or z
            if (dir == 'x')
            {
                nextPos.z = _currentPos.z;
            }
            else
            {
                nextPos.x = _currentPos.x;
            }

            float y = _currentPos.y;
            if (dir == 'x')
            {
                float min = Mathf.Min(_currentPos.x, nextPos.x);
                float max = Mathf.Max(_currentPos.x, nextPos.x);
                float z = _currentPos.z;
                for (float x = min; x <= (max + 0.1f); x += 1f)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject road = Instantiate(roadPrefabObj);
                    road.transform.position = pos;
                    _tempRoads.Add(road);
                }
            }
            else
            {
                float min = Mathf.Min(_currentPos.z, nextPos.z);
                float max = Mathf.Max(_currentPos.z, nextPos.z);
                float x = _currentPos.x;
                for (float z = min; z <= (max + 0.1f); z += 1f)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject road = Instantiate(roadPrefabObj);
                    road.transform.position = pos;
                    BuildingManager m = road.GetComponent<BuildingManager>();
                    m.SetPlacementMode(PlacementMode.Valid);
                    m.isFixed = false;
                    _tempRoads.Add(road);
                }
            }



            // Terraign Visibility Updates and placement validity
            validPlacement = true;
            int[] xPos = new int[_tempRoads.Count];
            int[] zPos = new int[_tempRoads.Count];
            for(int i = 0; i < xPos.Length; i++)
            {
                xPos[i] = Mathf.RoundToInt(_tempRoads[i].transform.position.x + 49.5f);
                zPos[i] = Mathf.RoundToInt(_tempRoads[i].transform.position.z + 49.5f);
                if (Map.mapBldgs[xPos[i]][zPos[i]] == eBuilding.none)
                {
                    Map.mapBldgs[xPos[i]][zPos[i]] = eBuilding.placeholder;
                }
                else
                {
                    validPlacement = false;
                }
            }
            terraignVisibilityManager.UpdateTerraignVisibility();
            for (int i = 0; i < xPos.Length; i++)
            {
                if (Map.mapBldgs[xPos[i]][zPos[i]] == eBuilding.placeholder)
                    Map.mapBldgs[xPos[i]][zPos[i]] = eBuilding.none;
            }

            // Set Valid Placement Visibility
            for (int i = 0; i < xPos.Length; i++)
            {
                Vector3 v = _tempRoads[i].transform.position;
                v.y += 0.01f;
                _tempRoads[i].transform.position = v;

                if (validPlacement)
                    _tempRoads[i].GetComponent<BuildingManager>().SetPlacementMode(PlacementMode.Valid);
                else
                    _tempRoads[i].GetComponent<BuildingManager>().SetPlacementMode(PlacementMode.Invalid);
            }

            if (Input.GetMouseButtonDown(0) && validPlacement)
            { // if left-click and the placement is valid.
                bool canBuild = true;
                foreach (GameObject road in _tempRoads)
                {
                    //turn to total cost
                    BuildingManager m = road.GetComponent<BuildingManager>();
                    if (!(m.hasValidPlacement && m.costWithinLimits))
                    {
                        canBuild = false;
                    }
                }
                if (canBuild)
                {
                    foreach (GameObject road in _tempRoads)
                    {
                        BuildingManager m = road.GetComponent<BuildingManager>();
                        m.SetPlacementMode(PlacementMode.Fixed);
                        m.placed = true;
                        m.isFixed = true;
                        _firstClick = true;
                        road.transform.parent = roadParent;
                    }
                    // Set height to normal
                    for (int i = 0; i < xPos.Length; i++)
                    {
                        Vector3 v = _tempRoads[i].transform.position;
                        v.y -= 0.02f;
                        _tempRoads[i].transform.position = v;
                    }

                    _tempRoads.Clear();

                    updateRoads = true;

                    for (int i = 0; i < xPos.Length; i++)
                    {
                        Map.mapBldgs[xPos[i]][zPos[i]] = eBuilding.road;
                    }
                }
            }
        }
        else
        {
            terraignVisibilityManager.UpdateTerraignVisibility();
        }
    }
    
    private Vector3 _ClampToNearest(Vector3 pos, float threshold)
    {
        float t = 1f / threshold;
        Vector3 v = ((Vector3)Vector3Int.FloorToInt(pos * t)) / t;

        // (recenter in middle of cells)
        float s = threshold * 0.5f;
        v.x += s;
        v.z += s;

        return v;
    }
}
