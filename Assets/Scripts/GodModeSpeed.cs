using UnityEngine;

public class GodModeSpeed : MonoBehaviour
{
    private Move move;
    private BatteryController _batteryController;
    void Start()
    {
        move = GetComponent<Move>();
        _batteryController = FindObjectOfType<BatteryController>();
    }
    
    void Update()
    {
        move.speed = _batteryController.isGod ? 10f : 5f;
    }
}
