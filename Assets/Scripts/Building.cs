using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public LayerMask groundLayer;

    private GameObject previewBuilding;
    private GameObject buildingsParent;
    private Renderer buildingRenderer;
    private BuildingStats collisionCheck;
    private bool canPlace = false;
    private Quaternion currentRotation = Quaternion.identity;
    private Material originalMaterial;
    public Material previewMaterial;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public float fixedY = 0.5f;
    public float gridSize = 1.0f;

    private RoadManager roadManager;

    void Start()
    {
        buildingsParent = GameObject.Find("Buildings") ?? new GameObject("Buildings");
        roadManager = FindFirstObjectByType<RoadManager>();
    }

    public void StartBuilding(GameObject newPrefab)
    {
        if (newPrefab == null) return;

        if (previewBuilding)
        {
            Destroy(previewBuilding);
            previewBuilding = null;
        }

        buildingPrefab = newPrefab;
        currentRotation = Quaternion.identity;
    }

    void OnDisable()
    {
        buildingPrefab = null;

        if (previewBuilding)
        {
            Destroy(previewBuilding);
            previewBuilding = null;
        }
    }

    void Update()
    {
        if (buildingPrefab == null)
        {
            enabled = false;
            return;
        }

        HandlePlacement();
        HandleRotation();
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPos = SnapToGrid(new Vector3(hit.point.x, fixedY, hit.point.z));

            if (previewBuilding == null)
            {
                previewBuilding = Instantiate(buildingPrefab, newPos, currentRotation);
                buildingRenderer = previewBuilding.GetComponentInChildren<Renderer>();
                collisionCheck = previewBuilding.GetComponentInChildren<BuildingStats>();
                originalMaterial = buildingRenderer.material;
                buildingRenderer.material = previewMaterial;
            }
            else
            {
                previewBuilding.transform.position = newPos;
                previewBuilding.transform.rotation = currentRotation;
            }

            if (previewBuilding != null)
            {
                Placement(previewBuilding.transform.position, previewBuilding.transform.rotation);
            }
        }
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0, 90, 0);
            if (previewBuilding != null)
            {
                previewBuilding.transform.rotation = currentRotation;
            }
        }
    }

    void Placement(Vector3 newPos, Quaternion newRot)
    {
        if (buildingPrefab.name == "DeletePreview")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUILayer()) return;
                {
                    collisionCheck.DeleteInBorders();

                }
            }
            return;
        }
        canPlace = collisionCheck == null || !collisionCheck.isOverlapping;
        buildingRenderer.material.color = canPlace ? validColor : invalidColor;



        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            if (IsPointerOverUILayer())
            {
                return;
            }
            GameObject placedBuilding = Instantiate(buildingPrefab, newPos, newRot);
            placedBuilding.transform.SetParent(buildingsParent.transform);
            placedBuilding.GetComponentInChildren<Renderer>().material = originalMaterial;
            if (placedBuilding.CompareTag("Road"))
            {
                roadManager.UpdateRoadAtPosition(placedBuilding, newPos);
            }

            Destroy(previewBuilding);
            previewBuilding = null;
        }
    }
    private bool IsPointerOverUILayer()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            position.y,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }
}
