using UnityEngine;
using UnityEngine.InputSystem;

//Script Made By Daniel Alvarado
[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public PlayerController controller;
    private Camera mainCamera;

    #region Events

    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    #endregion
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
        mainCamera = Camera.main;
        
    }

    private void Start()
    {
        controller.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        controller.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }
    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, controller.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
    }
    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, controller.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
    }
    public Vector2 PimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, controller.Touch.PrimaryPosition.ReadValue<Vector2>());
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
