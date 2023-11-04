using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//Script Made By Daniel Alvarado
public class Movement : MonoBehaviour
{
    private Vector3 direction;
    private CapsuleCollider playerCollider;
    private int desiredLane;
    private BatteryController batteryController;

    
    [SerializeField] private GameTimer gameTimer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isSliding = false;
    [HideInInspector] public bool shouldPlaySlideSpark = false;
    [HideInInspector] public bool isGodSliding;
    [HideInInspector] public bool isGodJumping = false;
    private bool isGamePaused = false;

    [Header("Movement Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float maxJumpForce = 35f;
    [SerializeField] private float minJumpForce = 30f;
    [Space]
    [SerializeField] private float slideTime = .5f;
    [Space]
    [SerializeField] private float currentJumpForce;
    [SerializeField] private float maxGravity = -40f;
    [SerializeField] private float minGravity = -30f;
    [SerializeField] private float currentGravity;
    [Space]
    [SerializeField] private float minSwitchSpeed = 5f;
    [SerializeField] private float maxSwitchSpeed = 10f;
    [SerializeField] private float currentSwitchSpeed;
    [Space]
    [SerializeField] private int numberOfLanes = 5;
    [SerializeField] private float laneWidth = 2f;
    [Space]
    [SerializeField] private float groundDistance;
    
    [Obsolete("Obsolete")]
    private void Awake()
    {
        batteryController = GetComponent<BatteryController>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        desiredLane = numberOfLanes / 2;
        playerInput.actions["Slide"].performed += Slide;
    }
    private void OnEnable()
    {
        playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        playerInput.actions.Disable();
    }


    private void Update()
    {
        if (!gameTimer.goingOn) return;

        AdjustGameSettingsBasedOnTimer();
        GroundCheck();
        MoveCharacter();
        if (!isSliding && rb.velocity.y < 0 && batteryController.isGod == false)
        {
            PlayerFXManager.Instance.DustEffect(); // This will ensure dust plays while grounded and not sliding
        }
        else if (batteryController.isGod)
        {
            PlayerFXManager.Instance.StopDustEffect();
        }
    }

    private void AdjustGameSettingsBasedOnTimer()
    {
        float progress = gameTimer.gameTimer / gameTimer.gameLength;
        currentGravity = Mathf.Lerp(minGravity, maxGravity, progress);
        currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, progress);
        currentSwitchSpeed = Mathf.Lerp(minSwitchSpeed, maxSwitchSpeed, progress);
        Physics.gravity = new Vector3(0, currentGravity, 0);
    }

    private void MoveCharacter()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        float lanePosition = (desiredLane - (numberOfLanes - 1) / 2.0f) * laneWidth;
        targetPosition += transform.right * lanePosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentSwitchSpeed * Time.deltaTime);
    }

    public void LaneTurn(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (!context.performed) return;
        desiredLane = Mathf.Clamp(desiredLane + (int)context.ReadValue<float>(), 0, numberOfLanes - 1);
            AudioManager.Instance.Play("Move_Woosh");
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (!context.performed || !isGrounded || !gameTimer.goingOn || isGamePaused) return;
        if(Time.timeScale == 1)
            AudioManager.Instance.Play("Jump");
        var velocity = rb.velocity;
        velocity = new Vector3(velocity.x, 0, velocity.z);
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        if (batteryController.isGod)
            StartCoroutine(JumpTimer());

        if (batteryController.isGod == false)
        {
            switch (isGrounded)
            {
                case true:
                    PlayerFXManager.Instance.DustEffect();
                    break;
                case false:
                    PlayerFXManager.Instance.StopDustEffect();
                    break;
            }
        }

    }
    
    private void GroundCheck()
    {
        RaycastHit hit;

        Vector3 dir = new Vector3(0, -1);

        bool wasGrounded = isGrounded; 

        if (Physics.Raycast(transform.position, dir, out hit, groundDistance))
        {
            isGrounded = true;
            if (wasGrounded || !shouldPlaySlideSpark) return; 
            PlayerFXManager.Instance.SlideSpark();
            shouldPlaySlideSpark = false; 
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator JumpTimer()
    {
        isGodJumping = true;
        yield return new WaitForSeconds(.5f);
        isGodJumping = false;
    }


    public void Slide(InputAction.CallbackContext context)
    {
        if (batteryController.isGod) return;
        if (Time.timeScale == 0) return;
        if (!context.performed || !gameTimer.goingOn || isSliding) return;

        StartCoroutine(SlideTimer());
        if (batteryController.isGod == false)
        {
            switch (isSliding)
            {
                case true:
                    PlayerFXManager.Instance.StopDustEffect();
                    break;
                case false:
                    PlayerFXManager.Instance.DustEffect();
                    break;
            }
        }

        if (!isGrounded) 
        {
            rb.AddForce(Vector3.down * currentJumpForce, ForceMode.Impulse);
            shouldPlaySlideSpark = true; 
        }
        else
        {
            PlayerFXManager.Instance.SlideSpark();
        }

        if(isSliding)
        {
            // When sliding starts, stop the dust effect
            PlayerFXManager.Instance.StopDustEffect();
        }

    }

    
    
    private IEnumerator SlideTimer()
    {
        if(this == null) yield break;

        float originalHeight = 2.8f;
        float slideHeight = 1f;

        Vector3 originalCenter = new Vector3(0, .33f, 0);
        Vector3 slideCenter = new Vector3(0, -.46f, 0);
        
        isSliding = true;

        playerCollider.height = slideHeight;
        playerCollider.center = slideCenter;

        AudioManager.Instance.Play("Slide");

        yield return new WaitForSeconds(slideTime);
    
        if(this == null) yield break;

        isSliding = false;

        playerCollider.center = originalCenter;
        playerCollider.height = originalHeight;
    }


    

    

    public void StepSound()
    {
        AudioManager.Instance.Play("StepSound");
    }
}


