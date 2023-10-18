using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 100f;

    //public Transform battery;
    void Start()
    {
        
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 30);
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
