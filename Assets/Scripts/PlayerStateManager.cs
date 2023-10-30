using System;
using Unity.VisualScripting;
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
        UpdateEffects();
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

    private void UpdateEffects()
    {
        
    }
}