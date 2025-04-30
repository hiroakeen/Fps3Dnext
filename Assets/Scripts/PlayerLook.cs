using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 1f;

    private PlayerInput input;
    private float xRotation = 0f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector2 lookInput = input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;

        // �����i�J���������j
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // �����i�v���C���[�{�́j
        transform.Rotate(Vector3.up * lookInput.x);
    }
}

