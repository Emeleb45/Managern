using UnityEngine;

public class ResidentialStats : MonoBehaviour
{
    private StatsManager statsManager;

    public Population population;
    public Electricity electricity;

    [System.Serializable]
    public struct Population
    {
        public int Current;
        public int Space;
    }
    [System.Serializable]
    public struct Electricity
    {
        public int Available;
        public int Cost;
    }
    [Header("Etc")]
    public int LandValue;
    public int CrimeRate;
    public int Pollution;
    public int FireHazard;

    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();

        statsManager.totalElectricity.Cost += electricity.Cost;
        statsManager.totalPopulation.Space += population.Space;
    }

    void OnDisable()
    {
        // Remove stats when disabled
        statsManager.totalElectricity.Cost -= electricity.Cost;
        statsManager.totalPopulation.Space -= population.Space;
    }
}
