using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target & Offset")]
    public Transform target;             
    public Vector3 offset;  

    [Header("Rotación")]
    public float sensitivity;        
    public float minY ;             
    public float maxY;              

    [Header("Colisión de Cámara")]
    public float collisionRadius;  
    public LayerMask collisionMask;       

    private float yaw = 0f;
    private float pitch = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPosition = target.position + rotation * offset;

        Vector3 direction = (desiredPosition - target.position).normalized;
        float maxDistance = offset.magnitude;

        RaycastHit hitInfo;
        if (Physics.SphereCast(target.position, collisionRadius, direction, out hitInfo, maxDistance, collisionMask))
        {
            float adjustedDistance = hitInfo.distance - 0.1f;
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
        return Quaternion.Euler(0f, yaw, 0f);
    }

    public Vector3 GetForward()
    {
        return Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
    }

    void OnDrawGizmos()
    {
        if (target == null) return;
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position + rot * offset;
        Vector3 dir = (desiredPos - target.position).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(target.position, dir * offset.magnitude);
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, collisionRadius);
    }

}
