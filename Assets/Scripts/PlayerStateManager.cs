using System;
using System.Collections;
using UnityEngine;

//Script Made By Daniel Alvarado
public class PlayerStateManager : MonoBehaviour
{
    public Animator animator;
    private GameTimer _gameTimer;
    private BatteryController _batteryController;

    public static PlayerStateManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    [Obsolete("Obsolete")]
    private void Start()
    {
        _batteryController = GetComponent<BatteryController>();
        _gameTimer = FindObjectOfType<GameTimer>();
    }

    private void Update()
    {
        UpdateAnimations();
        UpdateCharacterState();
        GodModeAnimations();
        //RunTurnAnimation();
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
    private static readonly int GodRight = Animator.StringToHash("GodRight");
    private static readonly int GodLight = Animator.StringToHash("GodLeft");
    private bool isGodTurning = false;
    private bool isTurnButtonReleased = true; 

    private void GodModeAnimations()
    {
        if (currentState != MovementState.GodMode) return;

        if (isGodTurning) return;

        float direction = InputManager.Instance.controller.Movement.Turn.ReadValue<float>();
        if (direction == 0)
        {
            isTurnButtonReleased = true;
            return;
        }

        if (!isTurnButtonReleased) return; 

        isGodTurning = true;
        isTurnButtonReleased = false; 
        animator.SetTrigger(direction > 0 ? GodRight : GodLight);
        StartCoroutine(ResetGodTurnFlagAfterSeconds(.1f));
    }


    private IEnumerator ResetGodTurnFlagAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isGodTurning = false;
    }
    
    // private bool isTurning = false;
    // private static readonly int Right = Animator.StringToHash("DefaultRightTurn");
    // private static readonly int Left = Animator.StringToHash("DefaultLeftTurn");
    // private void RunTurnAnimation()
    // {
    //     if(currentState != MovementState.Running) return;
    //     
    //     if(isTurning) return;
    //     
    //     float direction = InputManager.Instance.controller.Movement.Turn.ReadValue<float>();
    //     if (direction == 0) return;
    //     isTurning = true;
    //     animator.SetTrigger(direction > 0 ? Right : Left);
    //     StartCoroutine(ResetTurnFlagAfterSeconds(.1f));
    // }
    //
    // private IEnumerator ResetTurnFlagAfterSeconds(float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    //     isTurning = false;
    // }
}