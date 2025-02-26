using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public LayerMask groundLayer;

    private GameObject previewBuilding;
    private GameObject buildingsParent;
    private Renderer buildingRenderer;
    private BuildingCollision collisionCheck;
    private bool canPlace = false;

    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public float fixedY = 0.5f;

    private bool isRotating = false;

    void Start()
    {
        buildingsParent = GameObject.Find("Buildings");
        if (buildingsParent == null)
        {
            buildingsParent = new GameObject("Buildings");
        }
    }
    void OnDisable()
    {
        if (previewBuilding)
        {
            Destroy(previewBuilding);
        }
    }
    void Update()
    {
        if (isRotating)
        {
            HandleRotation();
        }
        else
        {
            HandlePlacement();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPos = new Vector3(hit.point.x, fixedY, hit.point.z);

            if (previewBuilding == null)
            {
                previewBuilding = Instantiate(buildingPrefab, newPos, Quaternion.identity);
                buildingRenderer = previewBuilding.GetComponentInChildren<Renderer>();
                collisionCheck = previewBuilding.GetComponentInChildren<BuildingCollision>();
            }
            else
            {
                previewBuilding.transform.position = newPos;
            }

            if (previewBuilding != null)
            {
                Placement(previewBuilding.transform.position, previewBuilding.transform.rotation);
            }
        }
    }

    void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookDirection = hit.point - previewBuilding.transform.position;
            lookDirection.y = 0;

            if (lookDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                previewBuilding.transform.rotation = Quaternion.Slerp(previewBuilding.transform.rotation, targetRotation, Time.deltaTime * 10);
            }
        }

        if (previewBuilding != null)
        {
            Placement(previewBuilding.transform.position, previewBuilding.transform.rotation);
        }
    }

    void Placement(Vector3 newPos, Quaternion newRot)
    {
        canPlace = !collisionCheck.isOverlapping;
        buildingRenderer.material.color = canPlace ? validColor : invalidColor;

        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            GameObject placedBuilding = Instantiate(buildingPrefab, newPos, newRot);
            placedBuilding.transform.SetParent(buildingsParent.transform); // Now sets the parent properly

            Destroy(previewBuilding);
            previewBuilding = null;
            isRotating = false;
        }
    }
}
