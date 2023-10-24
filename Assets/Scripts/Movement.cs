using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.VFX;

//Script Made By Daniel Alvarado
public class Movement : MonoBehaviour
{
    
    private GameTimer gameTimer;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector3 direction;
    private bool isJumping = false;
    private bool isSliding = false;
    private Collider playerCollider;
    public VisualEffect dustEffect;
    
    [Header("Movement")]
    [Space]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float laneSwitchSpeed = 10f;
    [Space]
    [Header("Animations & Effects")]
    [SerializeField] private Animator animator;
    [Space]
    [Header("Lanes")]
    [SerializeField] private int numberOfLanes = 5;
    [SerializeField] private float laneWidth = 2f; 
    private int desiredLane;
    [SerializeField] private float groundDistance;
    
    

    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        gameTimer = FindAnyObjectByType<GameTimer>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerInput.actions["Slide"].performed += Slide;
        desiredLane = numberOfLanes / 2;
    }

    private void OnDisable()
    {
        playerInput.actions["Slide"].performed -= Slide;
    }

    private void Update()
    {
        Animations();
        Physics.gravity = new Vector3(0, gravity, 0);
        GroundCheck();
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (gameTimer.goingOn)
        {
            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        
            float lanePosition = (desiredLane - (numberOfLanes - 1) / 2.0f) * laneWidth;

            targetPosition += transform.right * lanePosition;

            if (Time.timeScale != 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime / Time.timeScale);
            }
        }
    }


    public void LaneTurn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredLane = Mathf.Clamp(desiredLane + (int)context.ReadValue<float>(), 0, numberOfLanes - 1);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && gameTimer.goingOn)
        {
            
            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.velocity = velocity;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }
    }

    private bool isGrounded;
    void GroundCheck()
    {
        RaycastHit hit;
        
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, groundDistance))
        {
            isGrounded = true;
            dustEffect.Play();
        }
        else
        {
            dustEffect.Stop();
            isGrounded = false;
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && gameTimer.goingOn)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
    }

    private bool isRunning = false;
    private void Animations()
    {
        if (gameTimer.goingOn == false)
        {
            isRunning = false;
            animator.SetBool("IsIdle", true);
        }
    
        if (rb.velocity.y > 0)
        {
            isRunning = false;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSliding", false);
        }
        else if (isSliding)
        {
            isRunning = false;
            animator.SetBool("IsSliding", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsJumping", false);
        }
        else if (gameTimer.goingOn)
        {
            isRunning = true;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsSliding", false);
        }

        //if(dustEffect.)

    }

    public void FootStepSound()
    {
        if(isGrounded)
            AudioManager.instance.Play("StepSound");
    }

}


