using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    public GameObject straightRoadPrefab;
    public GameObject turnRoadPrefab;
    public GameObject tJunctionPrefab;
    public GameObject crossroadPrefab;

    private Dictionary<Vector3, GameObject> placedRoads = new Dictionary<Vector3, GameObject>();

    public void UpdateRoadAtPosition(GameObject newRoad, Vector3 position)
    {
        placedRoads[position] = newRoad;

        List<Vector3> directions = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1)
        };

        UpdateRoadType(position);

        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = position + dir;
            if (placedRoads.ContainsKey(neighborPos))
            {
                UpdateRoadType(neighborPos);
            }
        }
    }

    void UpdateRoadType(Vector3 position)
    {
        if (!placedRoads.ContainsKey(position)) return;

        GameObject road = placedRoads[position];
        if (road == null) return; 

        int connectionCount = 0;
        bool hasLeft = false, hasRight = false, hasForward = false, hasBackward = false;

        if (placedRoads.ContainsKey(position + new Vector3(1, 0, 0))) { connectionCount++; hasRight = true; }
        if (placedRoads.ContainsKey(position + new Vector3(-1, 0, 0))) { connectionCount++; hasLeft = true; }
        if (placedRoads.ContainsKey(position + new Vector3(0, 0, 1))) { connectionCount++; hasForward = true; }
        if (placedRoads.ContainsKey(position + new Vector3(0, 0, -1))) { connectionCount++; hasBackward = true; }

        GameObject newRoadPrefab = straightRoadPrefab;
        Quaternion newRotation = road.transform.rotation;

        if (connectionCount == 4)
        {
            newRoadPrefab = crossroadPrefab;
        }
        else if (connectionCount == 3)
        {
            newRoadPrefab = tJunctionPrefab;
            if (!hasLeft) newRotation = Quaternion.Euler(0, 0, 0);
            if (!hasRight) newRotation = Quaternion.Euler(0, 180, 0);
            if (!hasForward) newRotation = Quaternion.Euler(0, 90, 0);
            if (!hasBackward) newRotation = Quaternion.Euler(0, 270, 0);
        }
        else if (connectionCount == 2)
        {
            if ((hasLeft && hasRight))
            {
                newRoadPrefab = straightRoadPrefab;
                newRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (hasForward && hasBackward)
            {
                newRoadPrefab = straightRoadPrefab;
                newRotation = Quaternion.identity;
            }
            else
            {
                newRoadPrefab = turnRoadPrefab;
                if (hasLeft && hasForward) newRotation = Quaternion.Euler(0, 270, 0);
                if (hasRight && hasForward) newRotation = Quaternion.Euler(0, 0, 0);
                if (hasRight && hasBackward) newRotation = Quaternion.Euler(0, 90, 0);
                if (hasLeft && hasBackward) newRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (newRoadPrefab != road || road.transform.rotation != newRotation)
        {
            Vector3 pos = road.transform.position;
            Transform parent = road.transform.parent;


            placedRoads.Remove(position);
            Destroy(road);

            GameObject newRoad = Instantiate(newRoadPrefab, pos, newRotation);
            newRoad.transform.SetParent(parent);


            placedRoads[position] = newRoad;
        }
    }

}