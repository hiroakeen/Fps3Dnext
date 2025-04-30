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

        Vector2 moveInput = inputHandler.MoveInput;
        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        float speed = isRunning ? walkSpeed * runMultiplier : walkSpeed;

        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
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
            if (footstepTimer >= footstepInterval)
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