using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitOnEsc : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // エディタならプレイ停止
#else
        Application.Quit(); // ビルド版ならアプリ終了
#endif
    }
}
