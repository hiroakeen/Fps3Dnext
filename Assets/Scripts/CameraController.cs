using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _maincamera;
    [SerializeField] private CharacterController _characterController;

    [Header("Settings")]
    public float mouseSensitivity = 2.0f; // �}�E�X���x
    public Transform playerBody; // �v���C���[�{�̂�Transform�i�J�����̐e�j

    private float xRotation = 0f;
    private float zoomSpeed = 20f;
    private float normalFOV = 60f;
    private float aimFOV = 40f;
    public bool isAiming = false;

    // �w�b�h�{�u�p
    private float bobSpeed = 14f;
    private float bobAmount = 0.05f;
    private float defaultYPos;
    private float timer;

    void Start()
    {
        // �Q�[���J�n���Ƀ}�E�X�����b�N����\��
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
        // �}�E�X�ړ��ʂ��擾
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �㉺��]�i�J�����̂݁j
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ���_����90�x�A��90�x�ɐ���

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ���E��]�i�v���C���[�{�̂��񂷁j
        playerBody.Rotate(Vector3.up * mouseX);

        // ESC�L�[�Ń}�E�X���b�N�����i�f�o�b�O�p�j
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
