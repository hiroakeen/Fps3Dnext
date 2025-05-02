using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseInterval = 3f; // 3秒おきに
    [SerializeField] private float hungerDecreaseAmount = 1f;
    public float MaxHunger => maxHunger;
    public int Health { get; private set; }
    public float Hunger { get; private set; }
    public bool IsDead { get; private set; } = false;

    private float hungerTimer = 0f;

    void Awake()
    {
        Health = maxHealth;
        Hunger = maxHunger;
    }

    void Update()
    {
        if (IsDead) return;

        hungerTimer += Time.deltaTime;

        if (hungerTimer >= hungerDecreaseInterval)
        {
            Hunger -= hungerDecreaseAmount;
            Hunger = Mathf.Clamp(Hunger, 0f, maxHunger);
            hungerTimer = 0f;

            Debug.Log($"空腹度: {Hunger}");
        }

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

    public void RestoreHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, maxHealth);
    }

    void Die()
    {
        IsDead = true;
        GameManager.Instance.GameOver();
    }
}
