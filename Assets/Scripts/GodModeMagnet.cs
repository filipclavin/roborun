using System;
using UnityEngine;
//Script Made By Daniel Alvarado
public class GodModeMagnet : MonoBehaviour
{
    private BatteryController _batteryController;

    [SerializeField] private float distance = 15; 

    [Obsolete("Obsolete")]
    private void Start()
    {
        _batteryController = FindObjectOfType<BatteryController>();
    }
    
    private void Update()
    {
        if (!_batteryController.isGod) return;
        Vector3 directionToPlayer = Movement.Instance.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (!(distanceToPlayer <= distance)) return;
        const float speedModifier = 2.0f;

        directionToPlayer.Normalize();
        transform.position += directionToPlayer * (Time.deltaTime * 50f * speedModifier);
    }


}