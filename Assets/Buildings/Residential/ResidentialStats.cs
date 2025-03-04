using UnityEngine;
using System.Collections.Generic;

public class ResidentialStats : MonoBehaviour
{
    private StatsManager statsManager;
    public int populationSpace;
    public int ElectricityCost;

    public int population;
    public int LandValue;
    public int CrimeRate;
    public int Pollution;
    public int FireHazard;
    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.TotalElectricityCosts += ElectricityCost;
        statsManager.TotalPopulationSpace += populationSpace;
    }
    void OnDisable()
    {
        statsManager.TotalElectricityCosts -= ElectricityCost;
        statsManager.TotalPopulationSpace -= populationSpace;
    }
}
