using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

//Script Made By Daniel Alvarado
public class Movement : MonoBehaviour
{
    private GameTimer gameTimer;
    private Rigidbody rb;
    private Vector3 direction;
    private bool isSliding = false;
    private CapsuleCollider playerCollider;
    private int desiredLane;
    
    [Header("Movement")]
    [Space]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float maxJumpForce = 35f;
    [SerializeField] private float minJumpForce = 30f;
    [Space]
    [SerializeField] private float currentJumpForce;
    [Space]
    [SerializeField] private float maxGravity = -40f;
    [SerializeField] private float minGravity = -30f;
    [Space]
    [SerializeField] private float currentGravity;
    [Space]
    [SerializeField] private float minSwitchSpeed = 5f;
    [SerializeField] private float maxSwitchSpeed = 10f;
    [SerializeField] private float currentSwitchSpeed;
    
    [Space]
    [Header("Animations & Effects")]
    [SerializeField] private Animator animator;
    public List<ParticleSystem> effects;
    public VisualEffect dustEffect;
    [Space]
    [Header("Lanes")]
    [SerializeField] private int numberOfLanes = 5;
    [SerializeField] private float laneWidth = 2f; 
    [SerializeField] private float groundDistance;
    

    private void Start()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerInput.actions["Slide"].performed += Slide;
        desiredLane = numberOfLanes / 2;
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
    }

    private void Update()
    {
        currentSwitchSpeed = IncreaseLaneSwitchSpeed();
        Animations();
        IncreaseJumpForce();
        currentJumpForce = IncreaseJumpForce();
        var increaseGravity = IncreaseGravity();
        currentGravity = increaseGravity;
        Physics.gravity = new Vector3(0, increaseGravity, 0);
        GroundCheck();
        MoveCharacter();
    }

    private float IncreaseGravity()
    {
        if (gameTimer.goingOn)
        {
            float progress = gameTimer.gameTimer / gameTimer.gameLength;
            float adjustedGravity = Mathf.Lerp(minGravity, maxGravity, progress * 1);
           
            return adjustedGravity;
        }
        else
        {
            return minGravity;
        }
    }
    
    private float IncreaseJumpForce()
    {
        if (gameTimer.goingOn)
        {
            float progress = gameTimer.gameTimer / gameTimer.gameLength;
            float adjustedJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, progress * 1);
            
            return adjustedJumpForce;
        }
        else
        {
            return minJumpForce;
        }
    }


    private float IncreaseLaneSwitchSpeed()
    {
        if(gameTimer.goingOn)
        {
            float progress = gameTimer.gameTimer / gameTimer.gameLength;
            float increasedLaneSwitchSpeed = Mathf.Lerp(minSwitchSpeed, maxSwitchSpeed, progress * 1);
            
            return increasedLaneSwitchSpeed;
        }
        else
        {
            return minSwitchSpeed;
        }
    }
    private void MoveCharacter()
    {
        var increasedLaneSwitchSpeed = IncreaseLaneSwitchSpeed();
        if (gameTimer.goingOn)
        {
            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        
            float lanePosition = (desiredLane - (numberOfLanes - 1) / 2.0f) * laneWidth;

            targetPosition += transform.right * lanePosition;

            
            transform.position = Vector3.Lerp(transform.position, targetPosition, increasedLaneSwitchSpeed * Time.deltaTime);
            
        }
    }


    public void LaneTurn(InputAction.CallbackContext context)
    {
        if (context.performed && gameTimer.goingOn)
        {
            desiredLane = Mathf.Clamp(desiredLane + (int)context.ReadValue<float>(), 0, numberOfLanes - 1);
            FindObjectOfType<AudioManager>().Play("Move_Woosh");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        var adjustedJumpForce = IncreaseJumpForce();
        if (context.performed && isGrounded && gameTimer.goingOn)
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.velocity = velocity;

            rb.AddForce(Vector3.up * adjustedJumpForce, ForceMode.Impulse);

            StartCoroutine(DustTimer());
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
        }
        else
        {
            isGrounded = false;
        }
    }


    public void Slide(InputAction.CallbackContext context)
    {
        
        var adjustedJumpForce = IncreaseJumpForce();
        if (context.performed && gameTimer.goingOn && !isSliding)
        {
            rb.AddForce(Vector3.down * adjustedJumpForce, ForceMode.Impulse);
            StartCoroutine(SlideTimer());
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
    
        if (rb.velocity.y > 1 )
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
        else if (gameTimer.goingOn)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsSliding", false);
        }
    }

    private IEnumerator SlideTimer()
    {
        float originalHeight = 2.8f;
        float slideHeight = 1f;
        
        Vector3 originalCenter = new Vector3(0, .33f, 0);
        Vector3 slideCenter = new Vector3(0, -.46f, 0);
        
        isSliding = true;
        playerCollider.height = slideHeight;
        playerCollider.center = slideCenter;
        
        FindObjectOfType<AudioManager>().Play("Slide");
        
        yield return new WaitForSeconds(.5f);
        isSliding = false;
        playerCollider.center = originalCenter;
        playerCollider.height = originalHeight;
    }

    private IEnumerator DustTimer()
    {
        dustEffect.Stop();
        yield return new WaitForSeconds(1f);
        dustEffect.Play();
    }
    public void FootStepSound()
    {
        if(isGrounded)
            FindObjectOfType<AudioManager>().Play("StepSound");
    }

}


