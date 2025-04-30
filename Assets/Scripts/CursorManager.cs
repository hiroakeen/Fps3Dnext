using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "MainScene")
        {
            // �v���C�V�[�����̓J�[�\�������b�N����\��
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // �^�C�g���E�N���A�E�Q�[���I�[�o�[�ł̓J�[�\���\�������R
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
