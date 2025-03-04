using UnityEngine;
using System.Collections.Generic;

public class FireDepartmentStats : MonoBehaviour
{
    private StatsManager statsManager;

    public int FireProtection;
    public int jobPositions;
    public int Employees;
    public int ElectricityCost;


    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.TotalElectricityCosts += ElectricityCost;
        statsManager.TotalJobPositions += jobPositions;
    }
    void OnDisable()
    {
        statsManager.TotalElectricityCosts -= ElectricityCost;
        statsManager.TotalJobPositions -= jobPositions;
    }
}
