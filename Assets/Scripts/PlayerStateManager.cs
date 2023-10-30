using System;
using System.Collections;
using UnityEngine;

//Script Made By Daniel Alvarado
public class PlayerStateManager : MonoBehaviour
{
    public Animator animator;
    private GameTimer _gameTimer;
    private BatteryController _batteryController;
    private PlayerVisuals _playerVisuals;

    public static PlayerStateManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _playerVisuals = GetComponent<PlayerVisuals>();
        _batteryController = GetComponent<BatteryController>();
        _gameTimer = FindObjectOfType<GameTimer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimations();
        UpdateCharacterState();
        GodModeAnimations();
    }

    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Sliding,
        GodMode
    }

    private MovementState currentState;

    private const float JumpVelocity = 1;

    private void UpdateCharacterState()
    {
        if (!_gameTimer.goingOn)
        {
            currentState = MovementState.Idle;
            return;
        }

        if (Movement.Instance.rb.velocity.y > JumpVelocity)
        {
            currentState = MovementState.Jumping;
            return;
        }

        if (Movement.Instance.isSliding)
        {
            currentState = MovementState.Sliding;
            return;
        }

        if (_batteryController.isGod)
        {
            currentState = MovementState.GodMode;
            return;
        }


        currentState = MovementState.Running;
        
            
    }

    private void UpdateAnimations()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsSliding", false);
        animator.SetBool("IsGodMode", false);

        switch (currentState)
        {
            case MovementState.Idle:
                animator.SetBool("IsIdle", true);
                break;
            case MovementState.Jumping:
                animator.SetBool("IsJumping", true);
                break;
            case MovementState.Sliding:
                animator.SetBool("IsSliding", true);
                break;
            case MovementState.Running:
                animator.SetBool("IsRunning", true);
                break;
            case MovementState.GodMode:
                animator.SetBool("IsGodMode", true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private bool isTurning = false;

    private void GodModeAnimations()
    {
        if (currentState != MovementState.GodMode) return;

        if (isTurning) return; // Skip this turn if we're already turning

        float direction = InputManager.Instance.controller.Movement.Turn.ReadValue<float>();
        if (direction != 0)
        {
            isTurning = true; // Indicate that a turn is in progress
            if (direction > 0)
            {
                animator.SetTrigger("GodRight");
            }
            else
            {
                animator.SetTrigger("GodLeft");
            }

            // Assume your animation length is 1 second; adjust as needed
            StartCoroutine(ResetTurnFlagAfterSeconds(.1f));
        }
    }

    private IEnumerator ResetTurnFlagAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isTurning = false;
    }


    
}