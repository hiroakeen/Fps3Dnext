using UnityEngine;

public class GameExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // �G�f�B�^�[��ł̊m�F�p
#endif
        }
    }
}
