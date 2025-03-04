using UnityEngine;
using System.Collections.Generic;

public class CommerialStats : MonoBehaviour
{
    private StatsManager statsManager;
    public int jobPositions;
    public int ElectricityCost;

    public int Employees;
    public int LandValue;
    public int CrimeRate;
    public int Pollution;
    public int FireHazard;
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

