using UnityEngine;
using System.Collections.Generic;

public class RoadStats : MonoBehaviour
{
    private StatsManager statsManager;
    public int Condition = 100;
    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();

    }
}
