using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
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

    void Update()
    {
        if (inputHandler.IsAiming)
        {
            footstepTimer = 0f;
            playerAnimation.SetMoveSpeed(0f);
            return; // \‚¦’†‚ÍˆÚ“®•s‰Â
        }

        Vector2 moveInput = inputHandler.MoveInput;
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        float currentSpeed = move.magnitude;
        playerAnimation.SetMoveSpeed(currentSpeed);

        if (move != Vector3.zero)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                audioPlayer.PlayFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
