using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // The point the camera orbits around
    public float moveSpeed = 5f; // WASD movement speed

    public float distance = 10.0f; // Default zoom distance
    public float minDistance = 5f; // Min zoom distance
    public float maxDistance = 20f; // Max zoom distance
    public float zoomSpeed = 2f; // Speed of zooming

    public float rotationSpeed = 5.0f; // Rotation speed
    public float minYAngle = 10f; // Minimum vertical angle
    public float maxYAngle = 80f; // Maximum vertical angle

    private float yaw = 0.0f; // Horizontal rotation
    private float pitch = 30.0f; // Vertical rotation

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbit: No target assigned!");
            return;
        }

        // Initialize rotation angles
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        if (target == null) return;

        HandleMovement();
        HandleRotation();
        HandleZoom();
        UpdateCameraPosition();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"), // A/D or Left/Right for X-axis
            0,
            Input.GetAxis("Vertical")   // W/S or Up/Down for Z-axis
        );
        
        // Move relative to camera rotation (ignoring Y-axis)
        Vector3 moveDirection = Quaternion.Euler(0, yaw, 0) * move;
        target.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) 
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle); 
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
