using System;
using UnityEngine;

public class GodModeMagnet : MonoBehaviour
{
    private BatteryController _batteryController;

    [SerializeField] private float distance = 15; 

    void Start()
    {
        _batteryController = FindObjectOfType<BatteryController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_batteryController.isGod) return;
        Vector3 directionToPlayer = Movement.Instance.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= distance)
        {
            float speedModifier = 2.0f;

            directionToPlayer.Normalize();
            transform.position += directionToPlayer * (Time.deltaTime * 50f * speedModifier);
        }
    }


}

