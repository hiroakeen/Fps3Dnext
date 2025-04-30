using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    private int remaining;

    public void Initialize(int total)
    {
        remaining = total;
        UpdateUI();
    }

    public void Decrease()
    {
        remaining--;
        UpdateUI();
    }

    public int GetRemaining() => remaining;

    private void UpdateUI()
    {
        enemiesRemainingText.text = "‚Ä‚« ‚ ‚Æ " + remaining + "‚Ð‚«";
    }
}
