using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [Header("‰ñ•œ—Ê")]
    public float hungerRestore = 30f;
    public int healthRestore = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStatus>(out var player))
        {
            player.RestoreHunger(hungerRestore);
            if (healthRestore > 0)
            {
                player.RestoreHealth(healthRestore);
            }

            Destroy(gameObject);
        }
    }
}
