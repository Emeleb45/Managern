using UnityEngine;
using System.Collections.Generic;

public class IndustrialStats : MonoBehaviour
{
    private StatsManager statsManager;
    public Jobs jobs;
    public Electricity electricity;
    [System.Serializable]
    public struct Jobs
    {
        public int Available;
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
    public int PollutionOutput;
    public int FireHazard;
    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.totalElectricity.Cost += electricity.Cost;
        statsManager.totalJobs.Available += jobs.Space;
    }
    void OnDisable()
    {
        statsManager.totalElectricity.Cost -= electricity.Cost;
        statsManager.totalJobs.Available -= jobs.Space;
    }
}
