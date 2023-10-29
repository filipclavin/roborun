using UnityEngine;

//Script Made By Daniel Alvarado
public class Rotation : MonoBehaviour
{
   
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(90*Time.deltaTime, 90 * Time.deltaTime,90*Time.deltaTime );
    }
}
