using UnityEngine;
using TMPro;

public class SurvivalTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            float time = GameManager.Instance.SurvivalTime;

            int totalSeconds = Mathf.FloorToInt(time);
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            timeText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
