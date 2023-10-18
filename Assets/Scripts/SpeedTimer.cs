using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTimer : MonoBehaviour
{
    public float speedTimer = 0;
    private Movement movement;
    
    void Start()
    {
        movement = GetComponent<Movement>();    
    }

    // Update is called once per frame
    void Update()
    {
        speedTimer += 1 * Time.deltaTime;


        // if (!(movement.forwardSpeed <= 20)) return;
        // if (speedTimer % 5 < Time.deltaTime)
        // {
        //     movement.forwardSpeed += 2;
        // }
        
        while (speedTimer % 5 < Time.deltaTime)
        {
            movement.forwardSpeed += 2;
            break;
        }

    }
}
