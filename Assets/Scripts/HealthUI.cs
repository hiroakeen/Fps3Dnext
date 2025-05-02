using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Image[] hearts; // Heart1, Heart2, Heart3

    void Update()
    {
        int hp = playerStatus.Health;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < hp;
        }
    }
}
