using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [Header("Camera & Control")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float mouseSensitivity = 1.0f;

    [Header("Zoom Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float zoomSpeed = 20f;
    public bool isAiming = false;

    [Header("Head Bob Settings")]
    [SerializeField] private float bobSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;

<<<<<<< Updated upstream


=======
>>>>>>> Stashed changes
    private float defaultYPos;
    private float bobTimer;
    private float xRotation = 0f;

    private PlayerInput input;
    private PlayerShooting playerShooting;

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

<<<<<<< Updated upstream
        Vector2 mouseLook = input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;
        Vector2 combinedLook = mouseLook;
=======
        Vector2 lookInput = input.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;
>>>>>>> Stashed changes

        // 垂直回転（カメラのみ）
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

<<<<<<< Updated upstream
      
=======
        // 水平回転（プレイヤー本体）
        playerBody.Rotate(Vector3.up * lookInput.x);
>>>>>>> Stashed changes

        HandleAiming();
        HandleHeadBobbing();

        // ESCキーでマウス解放（デバッグ用）
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
        // 入力スライダーの値 (1〜10) を対数的に変換
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
