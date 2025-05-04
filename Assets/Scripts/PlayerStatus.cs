using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseInterval = 3f;
    [SerializeField] private float hungerDecreaseAmount = 1f;

    [Header("エフェクト")]
    [SerializeField] private BloodEffectController bloodEffect;

    public float MaxHunger => maxHunger;
    public int Health { get; private set; }
    public float Hunger { get; private set; }
    public bool IsDead { get; private set; }

    private float hungerTimer = 0f;

    void Awake()
    {
        Health = maxHealth;
        Hunger = maxHunger;
        IsDead = false;

        bloodEffect?.SetMaxHealth(maxHealth);
        bloodEffect?.SyncBloodWithHealth(Health);
    }

    void Update()
    {
        if (IsDead) return;
        HandleHunger();
    }

    private void HandleHunger()
    {
        hungerTimer += Time.deltaTime;

        if (hungerTimer >= hungerDecreaseInterval)
        {
            Hunger = Mathf.Clamp(Hunger - hungerDecreaseAmount, 0f, maxHunger);
            hungerTimer = 0f;
        }

        if (Hunger <= 0 || Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0) return;

        Health = Mathf.Clamp(Health - amount, 0, maxHealth);
        bloodEffect?.SyncBloodWithHealth(Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int amount)
    {
        if (IsDead || amount <= 0 || Health >= maxHealth) return;

        Health = Mathf.Clamp(Health + amount, 0, maxHealth);
        bloodEffect?.SyncBloodWithHealth(Health);
    }

    public void RestoreHunger(float amount)
    {
        if (IsDead || amount <= 0) return;

        Hunger = Mathf.Clamp(Hunger + amount, 0f, maxHunger);
    }

    private void Die()
    {
        IsDead = true;
        GameManager.Instance?.GameOver();
    }
}
