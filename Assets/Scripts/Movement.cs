using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [Space]
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float gravity = -20f;
    [SerializeField]
    private float laneWidth = 2f; 
    [SerializeField]
    private float laneSwitchSpeed = 10f;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int numberOfLanes = 5;
    private int desiredLane;

    private PlayerInput playerInput;
    private Rigidbody rb;

    private Vector3 direction;

    private bool isJumping = false;
    private bool isSliding = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        
        float lanePosition = (desiredLane - (numberOfLanes - 1) / 2.0f) * laneWidth;

        targetPosition += transform.right * lanePosition;

        if (Time.timeScale != 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime / Time.timeScale);
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
        if (context.performed && isGrounded)
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
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
    }

    private void Animations()
    {
        if (rb.velocity.y > 0)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSliding", false);
        }
        else if (isSliding)
        {
            animator.SetBool("IsSliding", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsSliding", false);
        }
    }
}
