using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;

    private int desiredLane = 1;
    public float laneDistance = 4f;

    public Rigidbody rb;

    private bool isSliding = false;
    private float lastTapTime = 0f;
    public GameObject mesh;

    private float directionTimer = 1f;

    public InputManager input;

    private bool isJumping = false;

    void Start()
    {
        
    }

    void Update()
    {
        direction.z = forwardSpeed;

        if (input.controller.Movement.Jump.triggered && controller.isGrounded)
        {
            isJumping = true;
            direction.y = -1;
            Jump();
        }
        else
        {
            isJumping = false;
            direction.y += gravity * Time.deltaTime;
        }

        if (isJumping == false)
        {
            if (input.controller.Movement.Right.WasPerformedThisFrame() && controller.isGrounded)
            {
                desiredLane++;
                if (desiredLane == 3)
                {
                    desiredLane = 2;
                }
            }

            if (input.controller.Movement.Left.WasPressedThisFrame() && controller.isGrounded)
            {
                desiredLane--;
                if (desiredLane == -1)
                {
                    desiredLane = 0;
                }
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 1 * Time.fixedDeltaTime);

        if (input.controller.Movement.Slide.triggered)
        {
            Slide();
            StartCoroutine(SlideTimer());
        }
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void Slide()
    {
        controller.height = 1f;
        mesh.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    private IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(1f);
        controller.height = 2f;
        mesh.transform.localScale = new Vector3(1, 1, 1);
    }
}
