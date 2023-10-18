using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject mesh;
    [SerializeField] private InputManager input;
    [Space]
    [Header("Movement")]
    public float forwardSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float laneDistance = 4f;
    [SerializeField] private float laneSwitchSpeed = 10f;
    
    private Vector3 direction;
    private int desiredLane = 1;
    private bool isJumping = false;
    private bool isSliding = false;

    private void Update()
    {
        HandleInput();
        MoveCharacter();
    }

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void HandleInput()
    {
        direction.z = forwardSpeed;

        if (input.controller.Movement.Jump.triggered && controller.isGrounded)
        {
            isJumping = true;
            direction.y = jumpForce;
        }
        else
        {
            isJumping = false;
            direction.y += gravity * Time.deltaTime;
        }

        LaneMovement();

        if (input.controller.Movement.Slide.triggered && controller.isGrounded)
        {
            Slide();
            StartCoroutine(SlideTimer());
        }
    }

    private void MoveCharacter()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        switch (desiredLane)
        {
            case 0:
                targetPosition += Vector3.left * laneDistance;
                break;
            case 2:
                targetPosition += Vector3.right * laneDistance;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
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
        mesh.transform.localScale = Vector3.one;
    }

    private void LaneMovement()
    {
        if (isJumping) return;

        if (input.controller.Movement.Right.triggered && controller.isGrounded)
        {
            desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2);
        }
        else if (input.controller.Movement.Left.triggered && controller.isGrounded)
        {
            desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2);
        }
    }
}
