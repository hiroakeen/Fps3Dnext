using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [Header("Camera & Control")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float mouseSensitivity = 1.0f;
    [SerializeField] private float smoothTime = 0.05f;

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
    private Vector2 currentLook;
    private Vector2 currentLookVelocity;
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
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;

        Vector2 mouseLook = input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;
        Vector2 combinedLook = mouseLook;

        currentLook = Vector2.SmoothDamp(currentLook, combinedLook, ref currentLookVelocity, smoothTime);

        xRotation -= currentLook.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * currentLook.x);

      

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
        value = Mathf.Clamp(value, 1f, 10f);
        float baseValue = 0.005f;
        float exponent = 2.2f;
        float scale = 0.003f;
        mouseSensitivity = baseValue + Mathf.Pow(value, exponent) * scale;
    }

    private bool PlayerIsDrawingBow()
    {
        return playerShooting != null && playerShooting.IsDrawing;
    }
}
