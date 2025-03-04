using UnityEngine;
using System.Collections.Generic;

public class IndustrialStats : MonoBehaviour
{
    private StatsManager statsManager;
    public int jobPositions;
    public int ElectricityCost;
    public int Employees;
    public int LandValue;
    public int CrimeRate;
    public int PollutionOutput;
    public int FireHazard;
    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.TotalElectricityCosts += ElectricityCost;
        statsManager.TotalJobPositions += jobPositions;
    }
}
