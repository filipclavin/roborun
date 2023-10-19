using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f;
    
    void Start()
    {
        
    }

    // Update is called once per framess
    void Update()
    {
        gameObject.transform.position += Vector3.back * (speed * Time.deltaTime);
    }
}
