using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseInterval = 3f;
    [SerializeField] private float hungerDecreaseAmount = 1f;

    [Header("UI演出")]
    [SerializeField] private Image damageMaskImage; // HPが減ると暗くなるマスク

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

        HandleHunger();
        UpdateDamageMask();
    }

    void HandleHunger()
    {
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
        Health = Mathf.Clamp(Health, 0, maxHealth);

        if (Health <= 0)
        {
            Die();
        }
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

    void UpdateDamageMask()
    {
        if (damageMaskImage == null) return;

        float healthRatio = Mathf.Clamp01((float)Health / maxHealth);
        float targetAlpha = 1f - healthRatio; // HPが減るほど濃く

        Color color = damageMaskImage.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * 5f);
        damageMaskImage.color = color;
    }
}
