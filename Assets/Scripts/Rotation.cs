using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
   
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(90*Time.deltaTime, 90 * Time.deltaTime,90*Time.deltaTime );
    }
}
