using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerAudio audioPlayer;
    private CharacterController controller;
    private PlayerInput input;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        audioPlayer = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (move != Vector3.zero)
        {
            audioPlayer.PlayFootstep();  // ‘«‰¹
        }
    }
}
