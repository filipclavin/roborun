using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    private Rigidbody rb;
    [Space]
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;
    public float slideTime = 1f;
    [SerializeField] private float laneDistance = 4f;
    [SerializeField] private float laneSwitchSpeed = 10f;
    [SerializeField] private Animator animator;
    
    private Vector3 direction;
    private int desiredLane = 1;
    private bool isJumping = false;
    private bool isSliding = false;
    
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        Physics.gravity = new Vector3(0, gravity, 0);
        GroundCheck();
        MoveForward();
        MoveCharacter();
    }

    private void MoveForward()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, forwardSpeed);
        
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

        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime / Time.timeScale);
    }

    public void LaneTurn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.ReadValue<float>() > 0)
            {
                desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2);
            }
            else if (context.ReadValue<float>() < 0)
            {
                desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2);
            }
        }
    }
    [SerializeField] private float jumpTimeScale = 1f;


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            jumpTimeScale = Time.timeScale;

            isJumping = true;

            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.velocity = velocity;

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            animator.SetBool("isJumping", true);
            animator.SetBool("isIdle", false);
            animator.speed = 1 / Time.timeScale;
        }
        else
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isJumping", false);
        }
    }

    private bool isGrounded;
    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if(Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    
}