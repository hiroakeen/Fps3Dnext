using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput input;


    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsRightClickHeld { get; private set; }
    public bool IsRightClickDown { get; private set; }
    public bool IsLeftClickDown { get; private set; }

    public bool IsAiming => IsRightClickHeld;



    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        MoveInput = input.actions["Move"].ReadValue<Vector2>();
        LookInput = input.actions["Look"].ReadValue<Vector2>();
        IsRightClickHeld = Mouse.current.rightButton.isPressed;
        IsRightClickDown = Mouse.current.rightButton.wasPressedThisFrame;
        IsLeftClickDown = Mouse.current.leftButton.wasPressedThisFrame;
    }
}
