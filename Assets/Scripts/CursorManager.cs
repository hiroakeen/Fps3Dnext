using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "MainScene")
        {
            // プレイシーン中はカーソルをロック＆非表示
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // タイトル・クリア・ゲームオーバーではカーソル表示＆自由
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
