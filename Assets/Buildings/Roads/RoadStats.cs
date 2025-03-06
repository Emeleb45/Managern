using UnityEngine;
using System.Collections.Generic;

public class RoadStats : MonoBehaviour
{
    private StatsManager statsManager;

    void OnEnable()
    {
        statsManager = FindFirstObjectByType<StatsManager>();

    }
    void OnDisable()
    {

    }
}
