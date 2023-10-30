using UnityEngine;
//Script Made By Daniel Alvarado
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
    private enum MovementState
    {
        Idle,
        Running,
        Jumping,
        Sliding
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

        currentState = MovementState.Running;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsSliding", false);

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
        }
    }
    
}
