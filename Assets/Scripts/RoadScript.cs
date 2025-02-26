using UnityEngine;
using System.Collections.Generic;

public class RoadPlacer : MonoBehaviour
{
    public GameObject roadPrefab;
    public LayerMask groundLayer;
    public float roadWidth = 1f;
    public float gridSize = 5f;
    public int curveResolution = 100;

    private GameObject roadsParent;
    private List<Vector3> roadNodes = new List<Vector3>();
    private bool isPlacing = false;
    private Vector3 gridCenter;

    void Start()
    {
        roadsParent = GameObject.Find("Manager/Roads");
        if (roadsParent == null)
        {
            Debug.LogError("Missing 'Roads' under 'Manager'! Please add an empty GameObject named 'Roads' inside 'Manager'.");
        }
    }

    void Update()
    {
        HandlePlacement();
    }

    void HandlePlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);


            if (Input.GetMouseButtonDown(0)) // Left-click to place nodes
            {
                PlaceNode(snappedPos);
            }
        }
    }

    void PlaceNode(Vector3 position)
    {
        if (roadNodes.Count == 0)
        {
            // First click: Set grid center
            gridCenter = position;
        }

        position = SnapToGrid(position); // Always snap

        roadNodes.Add(position);

        if (roadNodes.Count == 3) // We have 3 points, time to create the road
        {
            GenerateRoad();
            roadNodes.Clear(); // Reset for new road
        }
    }

    void GenerateRoad()
    {
        Vector3 p0 = roadNodes[0]; // First click
        Vector3 p1 = roadNodes[1]; // Second click (middle)
        Vector3 p2 = roadNodes[2]; // Third click (final point)
        if (p0 == p1)
        {
            return;
        }
        if (p1 != p2) // Only generate a curve if p1 and p2 are different
        {
            GenerateCurvedRoad(p0, p1, p2);
        }
        else
        {
            GenerateStraightRoad(p0, p1);
        }
    }



    void GenerateStraightRoad(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        GameObject roadSegment = Instantiate(roadPrefab, (start + end) / 2, Quaternion.LookRotation(end - start));
        roadSegment.transform.localScale = new Vector3(roadWidth, 1, distance);
        roadSegment.transform.SetParent(roadsParent.transform);
    }

    void GenerateCurvedRoad(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3[] curvePoints = GenerateBezierCurve(p0, p1, p2);

        for (int i = 0; i < curvePoints.Length - 1; i++)
        {
            GenerateStraightRoad(curvePoints[i], curvePoints[i + 1]);
        }
    }

    Vector3[] GenerateBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        List<Vector3> curve = new List<Vector3>();
        for (int i = 0; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 point = QuadraticBezier(p0, p1, p2, t);
            curve.Add(point);
        }
        return curve.ToArray();
    }

    Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round((position.x - gridCenter.x) / gridSize) * gridSize + gridCenter.x,
            0,
            Mathf.Round((position.z - gridCenter.z) / gridSize) * gridSize + gridCenter.z
        );
    }

    void ClearRoads()
    {
        foreach (Transform child in roadsParent.transform)
        {
            Destroy(child.gameObject);
        }
        roadNodes.Clear();
    }
}
