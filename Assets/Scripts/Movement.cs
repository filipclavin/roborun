using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [HideInInspector] public bool isGodSlising;
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
        playerInput.actions.Disable();
    }

    private void Update()
    {
        if (!gameTimer.goingOn) return;

        AdjustGameSettingsBasedOnTimer();
        GroundCheck();
        MoveCharacter();
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
        if (!context.performed) return;
        desiredLane = Mathf.Clamp(desiredLane + (int)context.ReadValue<float>(), 0, numberOfLanes - 1);
            
        if(Time.timeScale == 1)
            AudioManager.Instance.Play("Move_Woosh");
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGrounded || !gameTimer.goingOn || isGamePaused) return;
        if(Time.timeScale == 1)
            AudioManager.Instance.Play("Jump");
        var velocity = rb.velocity;
        velocity = new Vector3(velocity.x, 0, velocity.z);
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);

        if (batteryController.isGod == false)
        {
            StartCoroutine(DustTimer(1));
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




    public void Slide(InputAction.CallbackContext context)
    {
        if (!context.performed || !gameTimer.goingOn || isSliding) return;

        StartCoroutine(SlideTimer());
        if (batteryController.isGod == false)
        {
            StartCoroutine(DustTimer(.5f));
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
        if(!isSliding)
            PlayerFXManager.Instance.StopSlideSpark();
    }

    
    
    private IEnumerator SlideTimer()
    {
        float originalHeight = 2.8f;
        float slideHeight = 1f;
        
        Vector3 originalCenter = new Vector3(0, .33f, 0);
        Vector3 slideCenter = new Vector3(0, -.46f, 0);
        
        switch (batteryController.isGod)
        {
            case true:
                isGodSlising = true;
                break;
            case false:
                isSliding = true;
                break;
        }

        playerCollider.height = slideHeight;
        playerCollider.center = slideCenter;
        
        if(Time.timeScale == 1)
            AudioManager.Instance.Play("Slide");
        
        yield return new WaitForSeconds(slideTime);
        
        switch (batteryController.isGod)
        {
            case true:
                isGodSlising = false;
                break;
            case false:
                isSliding = false;
                break;
        }

        playerCollider.center = originalCenter;
        playerCollider.height = originalHeight;
    }

    private IEnumerator DustTimer(float waitTime)
    {
        PlayerFXManager.Instance.StopDustEffect();
        yield return new WaitForSeconds(waitTime);
        PlayerFXManager.Instance.DustEffect();
    }

    

    public void StepSound()
    {
        AudioManager.Instance.Play("StepSound");
    }
}


