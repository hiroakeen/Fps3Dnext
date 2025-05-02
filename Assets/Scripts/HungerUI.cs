using UnityEngine;
using UnityEngine.UI;

public class HungerUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Image hungerFillImage;

    void Update()
    {
        if (playerStatus != null && hungerFillImage != null)
        {
            hungerFillImage.fillAmount = playerStatus.Hunger / playerStatus.MaxHunger;

        }
    }
}
