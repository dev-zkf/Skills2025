using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputSystem_Actions Actions { get; private set; }

    // === Common Input Shortcuts ===
    public InputSystem_Actions.PlayerActions PlayerActions => Actions.Player;
    
    public Vector2 Move => PlayerActions.Move.ReadValue<Vector2>();
    public Vector2 Look => PlayerActions.Look.ReadValue<Vector2>();
    public bool JumpPressed => PlayerActions.Jump.triggered;
    public bool JumpReleased => PlayerActions.Jump.WasReleasedThisFrame();
    public bool JumpHeld => PlayerActions.Jump.ReadValue<float>() > 0.1f;

    public bool SprintHeld => PlayerActions.Sprint.ReadValue<float>() > 0.1f;

    public bool FirePressed => PlayerActions.Attack.triggered;
    public bool FireHeld => PlayerActions.Attack.ReadValue<float>() > 0.1f;

    public bool ReloadPressed => PlayerActions.Reload.triggered;
    public bool CrouchPressed => PlayerActions.Crouch.triggered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Actions = new InputSystem_Actions();
        Actions.Enable();

        DontDestroyOnLoad(gameObject); // Optional: if you want to persist between scenes
    }

    private void OnDisable()
    {
        Actions.Disable();
    }
}
