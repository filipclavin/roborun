using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float fixedZRotation = 15f;

    void Start()
    {
        // Initialize the rotation to the current rotation.
    }

    void Update()
    {
        // Get the current rotation.
        Quaternion currentRotation = transform.rotation;

        // Lock the Z rotation to the fixed value.
        currentRotation.z = fixedZRotation;

        // Calculate the new rotation only around the Y-axis.
        Quaternion yRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

        // Combine the new Y rotation with the locked Z rotation.
        Quaternion newRotation = yRotation * currentRotation;

        // Apply the new rotation to the object.
        transform.rotation = newRotation;
    }
}