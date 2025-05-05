using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runMultiplier = 2f;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 1f;

    [Header("Virtual Joystick")]
    [SerializeField] private VirtualJoystick leftJoystick;

    private CharacterController controller;
    private PlayerAudio audioPlayer;
    private PlayerInputHandler inputHandler;
    private PlayerAnimation playerAnimation;

    private float footstepTimer = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioPlayer = GetComponent<PlayerAudio>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (inputHandler.IsAiming)
        {
            footstepTimer = 0f;
            playerAnimation.SetMoveSpeed(0f);
            return;
        }

        Vector2 inputVector = inputHandler.MoveInput;
        if (leftJoystick != null)
        {
            Vector2 joystickInput = leftJoystick.InputDirection;
            if (joystickInput.sqrMagnitude > 0.01f)
                inputVector = joystickInput;
        }

        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        float speed = isRunning ? walkSpeed * runMultiplier : walkSpeed;

        Vector3 direction = transform.forward * inputVector.y + transform.right * inputVector.x;
        controller.Move(direction * speed * Time.deltaTime);

        float moveMagnitude = direction.magnitude;
        playerAnimation.SetMoveSpeed(moveMagnitude);

        HandleFootsteps(direction);
    }

    private void HandleFootsteps(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            footstepTimer += Time.deltaTime;
            bool isRunning = Keyboard.current.leftShiftKey.isPressed || RunButton.IsPressed;
            float currentInterval = isRunning ? 0.2f : footstepInterval;
            if (footstepTimer >= currentInterval)
            {
                audioPlayer?.PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
