using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private InputManager input;
    private Rigidbody rb;
    [Space]
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;
    public float slideTime = 1f;
    [SerializeField] private float laneDistance = 4f;
    [SerializeField] private float laneSwitchSpeed = 10f;
    
    
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
        HandleInput();
        MoveCharacter();
    }
    

    private void HandleInput()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, forwardSpeed);

        if (input.controller.Movement.Jump.triggered && isGrounded )
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            player.transform.localScale = Vector3.one;
        }

        LaneMovement();

        if (input.controller.Movement.Slide.triggered && isGrounded)
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
        player.transform.position = new Vector3(rb.position.x, rb.position.y -.5f, rb.position.z);
        player.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    private IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(slideTime);
        // var transformLocalPosition = rb.transform.localPosition;
        // transformLocalPosition.y = 0f;
        player.transform.localScale = Vector3.one;
        isSliding = false;
    }

    private void LaneMovement()
    {

        if (input.controller.Movement.Right.triggered )
        {
            desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2);
        }
        else if (input.controller.Movement.Left.triggered )
        {
            desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2);
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
