using UnityEngine;
using System.Collections.Generic;

public class PowerPlantStats : MonoBehaviour
{
    private StatsManager statsManager;
    public Jobs jobs;
    public ElectricityOutput electricityOutput;
    [System.Serializable]
    public struct Jobs
    {
        public int Available;
        public int Space;
    }
    [System.Serializable]
    public struct ElectricityOutput
    {
        public int Output;
    }



    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.totalElectricity.Available += electricityOutput.Output;
        statsManager.totalJobs.Available += jobs.Space;
    }
    void OnDisable()
    {
        statsManager.totalElectricity.Available -= electricityOutput.Output;
        statsManager.totalJobs.Available -= jobs.Space;
    }
}
