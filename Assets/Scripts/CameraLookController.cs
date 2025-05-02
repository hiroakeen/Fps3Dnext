using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [Header("Camera & Control")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float mouseSensitivity = 2.0f;

    [Header("Zoom Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float zoomSpeed = 20f;
    public bool isAiming = false;

    [Header("Head Bob Settings")]
    [SerializeField] private float bobSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;
    private float defaultYPos;
    private float bobTimer;
    private PlayerShooting playerShooting;

    private float xRotation = 0f;
    private PlayerInput input;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (characterController == null) characterController = GetComponentInParent<CharacterController>();
        playerShooting = GetComponent<PlayerShooting>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = cameraTransform.localPosition.y;
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused) return;

        Vector2 lookInput = input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;

        // 垂直回転（カメラ）
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 水平回転（プレイヤー本体）
        playerBody.Rotate(Vector3.up * lookInput.x);

        HandleAiming();
        HandleHeadBobbing();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleAiming()
    {
        float targetFOV = isAiming ? aimFOV : normalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, zoomSpeed * Time.unscaledDeltaTime);
    }

    private void HandleHeadBobbing()
    {
        if (characterController != null &&
            characterController.velocity.magnitude > 0.1f &&
            !isAiming &&
            !PlayerIsDrawingBow())
        {
            bobTimer += Time.deltaTime * bobSpeed;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                defaultYPos + Mathf.Sin(bobTimer) * bobAmount,
                cameraTransform.localPosition.z
            );
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                Mathf.Lerp(cameraTransform.localPosition.y, defaultYPos, Time.deltaTime * bobSpeed),
                cameraTransform.localPosition.z
            );
        }
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value * 0.5f; //感度の実感をあげるため0.5倍で調整
    }

    private bool PlayerIsDrawingBow()
    {
        return playerShooting != null && playerShooting.IsDrawing;
    }

}
