using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 2f;

    public int Health { get; private set; }
    public float Hunger { get; private set; }
    public bool IsDead { get; private set; } = false;

    void Awake()
    {
        Health = maxHealth;
        Hunger = maxHunger;
    }

    void Update()
    {
        if (IsDead) return;

        Hunger -= hungerDecreaseRate * Time.deltaTime;
        Hunger = Mathf.Clamp(Hunger, 0f, maxHunger);

        if (Hunger <= 0 || Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        Health -= amount;
        if (Health <= 0) Die();
    }

    public void RestoreHunger(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0, maxHunger);
    }

    void Die()
    {
        IsDead = true;
        GameManager.Instance.GameOver();
    }

    public void RestoreHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, maxHealth);
    }

}
