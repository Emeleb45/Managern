using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; 
    public float moveSpeed = 5f; 

    public float distance = 10.0f; 
    public float minDistance = 5f; 
    public float maxDistance = 20f; 
    public float zoomSpeed = 2f; 

    public float rotationSpeed = 5.0f; 
    public float minYAngle = 10f; 
    public float maxYAngle = 80f; 

    private float yaw = 0.0f; 
    private float pitch = 30.0f; 

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbit: No target assigned!");
            return;
        }


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
            Input.GetAxis("Horizontal"), 
            0,
            Input.GetAxis("Vertical")   
        );
        

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
