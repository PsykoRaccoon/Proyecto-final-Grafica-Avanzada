using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float sensitivity;
    public float minY;
    public float maxY;

    public float collisionRadius = 0.3f; 
    public LayerMask collisionMask;      


    private float yaw = 0;
    private float pitch = 10;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        Vector3 direction = (desiredPosition - target.position).normalized;
        float distance = offset.magnitude;

        RaycastHit hit;

        if (Physics.SphereCast(target.position, collisionRadius, direction, out hit, distance, collisionMask))
        {
            float adjustedDistance = hit.distance - 0.1f; 
            adjustedDistance = Mathf.Max(adjustedDistance, 0.1f); 

            transform.position = target.position + direction * adjustedDistance;
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(target);
    }


    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0, yaw, 0);
    }

    public Vector3 GetForward()
    {
        return Quaternion.Euler(0, yaw, 0) * Vector3.forward;
    }
}
