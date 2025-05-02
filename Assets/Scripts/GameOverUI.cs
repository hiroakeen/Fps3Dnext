using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    void Start()
    {
        float time = GameManager.Instance?.FinalSurvivalTime ?? 0f;
        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        resultText.text = $"ê∂ë∂éûä‘ÅF{minutes:D2}:{seconds:D2}";
    }

    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
