using UnityEngine;
using System.Collections.Generic;

public class BuildingCollision : MonoBehaviour
{

    [Header("BuildingStats")]
    public bool isOverlapping = false;
    public string buildingName;
    public int buildingPrice;
    private int collisionCount = 0;
    private List<GameObject> overlappingObjects = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Builds"))
        {
            collisionCount++;
            isOverlapping = collisionCount > 0;


            if (!overlappingObjects.Contains(other.gameObject))
            {
                overlappingObjects.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Builds"))
        {
            collisionCount--;
            isOverlapping = collisionCount > 0;

            overlappingObjects.Remove(other.gameObject);
        }
    }

    public void DeleteInBorders()
    {
        RoadManager roadManager = FindFirstObjectByType<RoadManager>();

        foreach (GameObject obj in new List<GameObject>(overlappingObjects))
        {
            if (obj.CompareTag("Road") && roadManager != null)
            {
                roadManager.RemoveRoadAtPosition(obj.transform.position);
            }

            Destroy(obj);
        }

        overlappingObjects.Clear();
        collisionCount = 0;
        isOverlapping = false;
    }

}
