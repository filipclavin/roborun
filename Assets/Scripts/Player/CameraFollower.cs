using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1f);
    }
}
