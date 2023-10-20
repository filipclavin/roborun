using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    void Update()
    {
        transform.position += 10f * Time.deltaTime * Vector3.back;
    }
}
