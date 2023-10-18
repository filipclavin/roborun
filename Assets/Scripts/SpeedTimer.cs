using UnityEngine;

public class SpeedTimer : MonoBehaviour
{
    public float speedTimer = 0;
    public float speedBoost = 2.5f;
    private Movement movement;
    
    void Start()
    {
        movement = GetComponent<Movement>();    
    }
    
    void Update()
    {
        speedTimer += 1 * Time.deltaTime;
        
        SpeedBoostTimer();
        
    }

    private void SpeedBoostTimer()
    {
        while (speedTimer % 5 < Time.deltaTime && movement.forwardSpeed < 20)
        {
            movement.forwardSpeed += speedBoost;
            break;
        }

        while (speedTimer % 10 < Time.deltaTime && movement.forwardSpeed is < 30 and >= 20)
        {
            movement.jumpForce = 30;
            movement.gravity = -30;
            movement.forwardSpeed += speedBoost;
            break;
        }
    }
}
