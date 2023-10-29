using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//Script Made By Daniel Alvarado
public class Movement : MonoBehaviour
{
    private Vector3 direction;
    private CapsuleCollider playerCollider;
    private int desiredLane;
    
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isSliding = false;

    [Header("Movement Settings")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float maxJumpForce = 35f;
    [SerializeField] private float minJumpForce = 30f;
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
    [Space]
    private float slideTime = .5f;
    private float currentSlideTime;
    [Space]
    
    private GameTimer gameTimer;
    
    public static Movement Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
        gameTimer = FindObjectOfType<GameTimer>();
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
        if (context.performed)
        {
            desiredLane = Mathf.Clamp(desiredLane + (int)context.ReadValue<float>(), 0, numberOfLanes - 1);
            AudioManager.Instance.Play("Move_Woosh");
        }
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && gameTimer.goingOn)
        {
            AudioManager.Instance.Play("Jump");
            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            rb.velocity = velocity;

            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);

            StartCoroutine(DustTimer());
        }
    }
    
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
        
        
        if (context.performed && gameTimer.goingOn && !isSliding)
        {
            rb.AddForce(Vector3.down * currentJumpForce, ForceMode.Impulse);
            StartCoroutine(SlideTimer());
        }
        
    }
    
    private bool isRunning = false;
    
    private IEnumerator SlideTimer()
    {
        float originalHeight = 2.8f;
        float slideHeight = 1f;
        
        Vector3 originalCenter = new Vector3(0, .33f, 0);
        Vector3 slideCenter = new Vector3(0, -.46f, 0);
        
        isSliding = true;
        playerCollider.height = slideHeight;
        playerCollider.center = slideCenter;
        
        AudioManager.Instance.Play("Slide");
        
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
        playerCollider.center = originalCenter;
        playerCollider.height = originalHeight;
    }

    private IEnumerator DustTimer()
    {
        PlayerFXManager.Instance.DustEffect();
        yield return new WaitForSeconds(1f);
        PlayerFXManager.Instance.StopDustEffect();
    }
    

}


