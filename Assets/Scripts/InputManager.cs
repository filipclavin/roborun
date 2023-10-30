using UnityEngine;
//Script Made By Daniel Alvarado
public class InputManager : MonoBehaviour
{
    public PlayerController controller;
    
    public static InputManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
