using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    private BuildingPlacer buildingPlacer;

    public GameObject[] buildingPrefabs;

    void Start()
    {
        buildingPlacer = GetComponent<BuildingPlacer>();
        buildingPlacer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            StopBuilding();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartBuilding(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartBuilding(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartBuilding(2);
        }

    }


    public void StartBuilding(int prefabIndex)
    {
        if (prefabIndex >= 0 && prefabIndex < buildingPrefabs.Length)
        {
            buildingPlacer.enabled = true;
            buildingPlacer.StartBuilding(buildingPrefabs[prefabIndex]);
        }
    }
    public void StopBuilding()
    {
        buildingPlacer.enabled = false;
    }

}
