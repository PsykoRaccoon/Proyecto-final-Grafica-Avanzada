using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CameraOrbit cameraOrbit;
    public float moveSpeed;
    public float rotationSpeed;
    public float runSpeed;
    public bool isAiming = false; // Simulado con click derecho, por ejemplo

    private CharacterController controller;
    private float rotationVelocity;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        HandleAiming();
    }

    void HandleAiming()
    {
        isAiming = Input.GetMouseButton(1);
        animator.SetBool("IsAiming", isAiming);
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        Quaternion camRotation = cameraOrbit.GetRotation();
        Vector3 moveDir = camRotation * inputDirection;

        float currentSpeed = 0f;

        if (inputDirection.magnitude >= 0.1f)
        {
            bool isRunning = !isAiming && Input.GetKey(KeyCode.LeftShift);
            currentSpeed = isRunning ? 2f : 1f;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            float moveSpeedToUse = isRunning ? runSpeed : moveSpeed;
            controller.Move(moveDir.normalized * moveSpeedToUse * Time.deltaTime);
        }
        else
        {
            Vector3 lookDirection = cameraOrbit.GetForward();
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            currentSpeed = 0f;
        }

        animator.SetFloat("Speed", currentSpeed);
    }
}
