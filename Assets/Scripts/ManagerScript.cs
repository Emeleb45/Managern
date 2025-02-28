using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    private BuildingPlacer buildingPlacer;

    public GameObject[] ResidentialPrefabs;
    public GameObject[] RoadPrefabs;
    public GameObject[] DevPrefabs;
    public GameObject buildCard;
    public GameObject BuildSelection;
    public GameObject BuildMenuLayout;
    private BuildingStats buildingStats;
    [Header("Buttons")]
    public Button ResidentialButton;
    public Button RoadButton;
    public Button DeleteButton;

    void Start()
    {
        buildingPlacer = GetComponent<BuildingPlacer>();
        buildingPlacer.enabled = false;
        ResidentialButton.onClick.AddListener(() => OpenBuildMenu("Residential"));
        RoadButton.onClick.AddListener(() => OpenBuildMenu("Road"));
        DeleteButton.onClick.AddListener(() => StartBuilding(0, DevPrefabs));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBuilding();
        }
    }

    public void OpenBuildMenu(string type)
    {
        foreach (Transform child in BuildMenuLayout.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject[] prefabs = null;

        if (type == "Residential")
        {
            prefabs = ResidentialPrefabs;
        }
        else if (type == "Road")
        {
            prefabs = RoadPrefabs;
        }

        if (prefabs != null)
        {
            int index = 0;
            foreach (GameObject prefab in prefabs)
            {
                buildingStats = prefab.GetComponent<BuildingStats>();
           
                GameObject card = Instantiate(buildCard, BuildMenuLayout.transform);

                Button cardButton = card.GetComponent<Button>();
                int capturedIndex = index; 
                cardButton.onClick.AddListener(() => StartBuilding(capturedIndex, prefabs));

                TextMeshProUGUI nameText = card.transform.Find("Label").GetComponent<TextMeshProUGUI>();
                nameText.text = buildingStats.buildingName;
                TextMeshProUGUI PropertyText = card.transform.Find("Stats").GetComponent<TextMeshProUGUI>();
                PropertyText.text = "Cost: " + buildingStats.cost + "\n" + "Electricity Cost: " + buildingStats.ElectricityCost;
                index++;
                buildingStats = null;
            }
        }
        BuildSelection.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        BuildSelection.SetActive(false);
        foreach (Transform child in BuildMenuLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartBuilding(int prefabIndex, GameObject[] Type)
    {
        if (BuildSelection.activeSelf)
        {
            CloseBuildMenu();
        }

        if (prefabIndex >= 0 && prefabIndex < Type.Length)
        {
            buildingPlacer.enabled = true;
            buildingPlacer.StartBuilding(Type[prefabIndex]);
        }
    }

    public void StopBuilding()
    {
        buildingPlacer.enabled = false;
    }
}