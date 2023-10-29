using UnityEngine;
//Script Made By Daniel Alvarado
public class InputManager : MonoBehaviour
{
    public PlayerController controller;
    void Awake()
    {
        controller = new PlayerController();
    }
    

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
}
