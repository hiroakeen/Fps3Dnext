using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float survivalTime = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log($"Game Over! ¶‘¶ŠÔ: {survivalTime:F1} •b");

        // —áFƒV[ƒ“Ä“Ç‚İ‚İ or GameOver‰æ–Ê‘JˆÚ
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
