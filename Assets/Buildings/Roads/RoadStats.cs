using UnityEngine;
using System.Collections.Generic;

public class RoadStats : MonoBehaviour
{
    private StatsManager statsManager;
    public int MaintenanceCost;
    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();
        statsManager.TotalExpenses += MaintenanceCost;
    }
    void OnDisable()
    {
        statsManager.TotalExpenses -= MaintenanceCost;
    }
}
