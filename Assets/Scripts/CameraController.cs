using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _maincamera;
    [SerializeField] private CharacterController _characterController;

    [Header("Settings")]
    public float mouseSensitivity = 2.0f; // マウス感度
    public Transform playerBody; // プレイヤー本体のTransform（カメラの親）

    private float xRotation = 0f;
    private float zoomSpeed = 20f;
    private float normalFOV = 60f;
    private float aimFOV = 40f;
    public bool isAiming = false;

    // ヘッドボブ用
    private float bobSpeed = 14f;
    private float bobAmount = 0.05f;
    private float defaultYPos;
    private float timer;

    void Start()
    {
        // ゲーム開始時にマウスをロック＆非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _maincamera = GetComponent<Camera>();
        if (_characterController == null)
        {
            _characterController = GetComponentInParent<CharacterController>();
        }
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        // マウス移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 上下回転（カメラのみ）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 視点を上90度、下90度に制限

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 左右回転（プレイヤー本体も回す）
        playerBody.Rotate(Vector3.up * mouseX);

        // ESCキーでマウスロック解除（デバッグ用）
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        HandleAiming();
    }

    private void HandleAiming()
    {
        float aiming = isAiming ? aimFOV : normalFOV;
        _maincamera.fieldOfView = Mathf.Lerp(_maincamera.fieldOfView, aiming, zoomSpeed * Time.deltaTime);
    }

    public void HandleHeadBobbing()
    {
        if (_characterController != null && _characterController.velocity.magnitude > 0.1f && !isAiming)
        {
            timer += Time.deltaTime * bobSpeed;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * bobAmount,
                transform.localPosition.z
            );
        }
        else
        {
            timer = 0f;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * bobSpeed),
                transform.localPosition.z
            );
        }
    }
}
