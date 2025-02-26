using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    private BuildingPlacer buildingPlacer;
    private RoadPlacer roadPlacer;

    private enum Mode { None, Building, Road }
    private Mode currentMode = Mode.None;

    void Start()
    {
        buildingPlacer = GetComponent<BuildingPlacer>();
        roadPlacer = GetComponent<RoadPlacer>();

        // Ensure both are disabled initially
        buildingPlacer.enabled = false;
        roadPlacer.enabled = false;
    }

    void Update()
    {
        HandleModeSwitch();
    }

    void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press "1" for Buildings
        {
            SetMode(Mode.Building);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press "2" for Roads
        {
            SetMode(Mode.Road);
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote)) // Press "`" to disable both
        {
            SetMode(Mode.None);
        }
    }

    void SetMode(Mode mode)
    {
        currentMode = mode;

        // Enable/Disable Components based on mode
        buildingPlacer.enabled = (currentMode == Mode.Building);
        roadPlacer.enabled = (currentMode == Mode.Road);
    }
}
