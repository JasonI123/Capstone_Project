using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingGridPlacer : MonoBehaviour
{
    private static bool[] buildErrors = { false, false, false, false, false};

    public float cellSize;
    public Renderer gridRenderer;
    public RoadPlacement roadPlacer;
    public TerraignVisibilityManager terraignVisibilityManager;
    public BuildButtonsHandler buildButtonsHandler;

    public LayerMask roadLayerMask;
    public LayerMask groundLayerMask;

    protected GameObject _buildingPrefab;
    private GameObject _toBuild;

    protected Camera _mainCamera;

    public static BuildingGridPlacer instance;

    protected Ray _ray;
    protected RaycastHit _hit;

    public GameObject roadPrefab;
    public Transform buildingGo;

    public static eBiome bBiome;
    public static bool validBiome;

    private void Awake()
    {
        instance = this;
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Start()
    {
        _UpdateGridVisual();
        _EnableGridVisual(false);
    }

    private void Update()
    {
        if (_buildingPrefab == roadPrefab)
        {
            Destroy(_toBuild);
            _toBuild = null;
            roadPlacer.placeRoad = true;
            buildButtonsHandler.SetBuildErrorText();

            // right-click: cancel build mode
            if (Input.GetMouseButtonDown(1))
            {
                buildButtonsHandler.SetBuildErrorText(true);
                roadPlacer.placeRoad = false;
                _EnableGridVisual(false);
                Destroy(_toBuild);
                _toBuild = null;
                _buildingPrefab = null;
                terraignVisibilityManager.UpdateTerraignVisibility();
                return;
            }
        }
        else if (_buildingPrefab != null)
        { // if in build mode
            roadPlacer.placeRoad = false;

            // right-click: cancel build mode
            if (Input.GetMouseButtonDown(1))
            {
                buildButtonsHandler.ClearBuildBiomeText();
                buildButtonsHandler.SetBuildErrorText(true);
                _EnableGridVisual(false);
                Destroy(_toBuild);
                _toBuild = null;
                _buildingPrefab = null;
                terraignVisibilityManager.UpdateTerraignVisibility();
                return;
            }

            bool buildingPresent = true;

            // hide preview when hovering UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                buildButtonsHandler.ClearBuildBiomeText();
                buildButtonsHandler.SetBuildErrorText(true);
                if (_toBuild.activeSelf) _toBuild.SetActive(false);
                terraignVisibilityManager.UpdateTerraignVisibility();
                return;
            }
            else
            {
                if (!_toBuild.activeSelf)
                    _toBuild.SetActive(true);

                buildButtonsHandler.SetBuildBiomeText(bBiome, validBiome);
                buildButtonsHandler.SetBuildErrorText();
            }

            // rotate preview with Spacebar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _toBuild.transform.Rotate(Vector3.up, 90);
            }

            // handle raycast for phantom building placement
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, groundLayerMask))
            {
                if (!_toBuild.activeSelf)
                    _toBuild.SetActive(true);

                _toBuild.transform.position = _ClampToNearest(_hit.point, cellSize);
                Vector3 temp = _toBuild.transform.position;
                temp.y += 0.02f;
                _toBuild.transform.position = temp;

                BuildingManager m = _toBuild.GetComponent<BuildingManager>();

                // Terraign Visibility Update
                int x = Mathf.RoundToInt(temp.x + 49.5f);
                int z = Mathf.RoundToInt(temp.z + 49.5f);
                if (buildingPresent)
                {

                    if (Map.mapBldgs[x][z] == eBuilding.none)
                    {
                        Map.mapBldgs[x][z] = eBuilding.placeholder;
                    }

                    foreach (BuildingBase b in m.subBuildings)
                    {
                        if (Map.mapBldgs[b.x][b.z] == eBuilding.none)
                            Map.mapBldgs[b.x][b.z] = eBuilding.placeholder;
                    }

                    terraignVisibilityManager.UpdateTerraignVisibility();

                    if (Map.mapBldgs[x][z] == eBuilding.placeholder)
                    {
                        Map.mapBldgs[x][z] = eBuilding.none;
                    }

                    foreach (BuildingBase b in m.subBuildings)
                    {
                        if (Map.mapBldgs[b.x][b.z] == eBuilding.placeholder)
                            Map.mapBldgs[b.x][b.z] = eBuilding.none;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                { // if left-click and the building is on a road.
                    if (m.hasValidPlacement && m.costWithinLimits)
                    {
                        JobSource.recentPlace = true;
                        BuildingBase.recentPlace = true;
                        m.SetPlacementMode(PlacementMode.Fixed);
                        m.placed = true;

                        Map.mapBldgs[x][z] = _toBuild.GetComponent<BuildingBase>().meBuilding;
                        Map.mapBldRot[x][z] = _toBuild.transform.rotation.eulerAngles.y;

                        foreach (BuildingBase b in m.subBuildings)
                        {
                            Map.mapBldgs[b.x][b.z] = eBuilding.subbuilding;
                            b.isFixedSubbuilding = true;
                        }

                        _toBuild = null; // (to avoid destruction)
                        _PrepareBuilding();
                    }
                }

            }
            else if (_toBuild.activeSelf)
            {
                _toBuild.SetActive(false);
                roadPlacer.placeRoad = false;
                terraignVisibilityManager.UpdateTerraignVisibility();
                buildButtonsHandler.ClearBuildBiomeText();
                buildButtonsHandler.SetErrorTextOutOfBounds();
            }
        }
    }

    private void _EnableGridVisual(bool on)
    {
        if (gridRenderer == null) return;
        gridRenderer.gameObject.SetActive(on);
    }
    private void _UpdateGridVisual()
    {
        if (gridRenderer == null) return;
        gridRenderer.sharedMaterial.SetVector(
        "_Cell_Size", new Vector4(cellSize, cellSize, 0, 0));
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

    private void _PrepareBuilding()
    {
        if (_toBuild) Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);
        //_toBuild.SetActive(false);

        BuildingManager m = _toBuild.GetComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
        _EnableGridVisual(true);
    }

    public void SetBuildingPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        _PrepareBuilding();
        EventSystem.current.SetSelectedGameObject(null); // cancel keyboard UI nav
    }

    public void CancelBuilding()
    {
        roadPlacer.placeRoad = false;
        _EnableGridVisual(false);
        Destroy(_toBuild);
        _toBuild = null;
        _buildingPrefab = null;
        terraignVisibilityManager.UpdateTerraignVisibility();
        return;
    }

    public static void SetBuildError(BuildError error, bool val)
    {
        buildErrors[(int)error] = val;
    }

    public static bool GetBuildError(BuildError error)
    {
        return buildErrors[(int)error];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _UpdateGridVisual();
    }
#endif
}

public enum BuildError
{
    overlapingItems,
    cantAfford,
    invalidLocation,
    notEnoughMaterials,
    notOnRoad,
}
