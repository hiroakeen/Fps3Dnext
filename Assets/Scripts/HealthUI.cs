using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Image[] hearts; 

    void Update()
    {
        if (playerStatus == null) return;

        int hp = Mathf.Clamp(playerStatus.Health, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < hp;
        }
    }
}
