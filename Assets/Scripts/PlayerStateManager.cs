using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private GameTimer _gameTimer;
    void Start()
    {
        _gameTimer = FindObjectOfType<GameTimer>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        UpdateAnimations();
        UpdateCharacterState();
    }
    private enum CharacterState
    {
        Idle,
        Running,
        Jumping,
        Sliding
    }

    private CharacterState currentState;
    
    private const float jumpVelocity = 1; 

    private void UpdateCharacterState()
    {
        if (!_gameTimer.goingOn)
        {
            currentState = CharacterState.Idle;
            return;
        }

        if (Movement.Instance.rb.velocity.y > jumpVelocity)
        {
            currentState = CharacterState.Jumping;
            return;
        }

        if (Movement.Instance.isSliding)
        {
            currentState = CharacterState.Sliding;
            return;
        }

        currentState = CharacterState.Running;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsSliding", false);

        switch (currentState)
        {
            case CharacterState.Idle:
                animator.SetBool("IsIdle", true);
                break;
            case CharacterState.Jumping:
                animator.SetBool("IsJumping", true);
                break;
            case CharacterState.Sliding:
                animator.SetBool("IsSliding", true);
                break;
            case CharacterState.Running:
                animator.SetBool("IsRunning", true);
                break;
        }
    }
    
}
