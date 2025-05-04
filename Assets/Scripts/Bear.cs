using UnityEngine;
using UnityEngine.AI;

public class Bear : AnimalBase, IDamageable
{
    [Header("Bear Settings")]
    public float detectRange = 15f;
    public float attackRange = 3f;
    public float sitChance = 0.05f;
    public float sitDuration = 10f;
    [SerializeField] private AudioClip bearCallSound;
    [SerializeField] private GameObject foodPrefab;

    private Transform player;
    private bool isDead = false;
    private bool isSitting = false;
    private bool inCombat = false;

    private float sitTimer = 0f;
    private float randomCheckTimer = 0f;
    private readonly string[] attackTriggers = { "Attack1", "Attack2", "Attack3" };

    private bool hasReacted = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        callSound = bearCallSound;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            if (!hasReacted)
            {
                ShowAlertIcon(); // ← 気づいたときに一度だけ表示
                hasReacted = true;
            }
            EnterCombat();
        }
        else
        {
            ExitCombat();
            hasReacted = false;

            if (!isSitting)
            {
                RandomBehavior();
            }
        }

        if (isSitting)
        {
            sitTimer += Time.deltaTime;
            if (sitTimer >= sitDuration)
            {
                isSitting = false;
                animator.SetBool("Sit", false);
            }
        }
    }

    void RandomBehavior()
    {
        randomCheckTimer += Time.deltaTime;
        if (randomCheckTimer >= 5f)
        {
            if (Random.value < sitChance)
            {
                Sit();
            }
            randomCheckTimer = 0f;
        }
    }

    void Sit()
    {
        isSitting = true;
        sitTimer = 0f;
        agent.ResetPath();
        animator.SetBool("Sit", true);
    }

    void EnterCombat()
    {
        if (isSitting || inCombat) return;

        inCombat = true;
        agent.ResetPath();
        animator.SetBool("CombatIdle", true);
        InvokeRepeating(nameof(TryAttack), 1.5f, Random.Range(2f, 3.5f));
    }

    void ExitCombat()
    {
        if (!inCombat) return;

        inCombat = false;
        animator.SetBool("CombatIdle", false);
        CancelInvoke(nameof(TryAttack));
    }

    void TryAttack()
    {
        if (!inCombat || isSitting || isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange)
        {
            string selectedTrigger = attackTriggers[Random.Range(0, attackTriggers.Length)];
            animator.SetTrigger(selectedTrigger);

            if (player.TryGetComponent<PlayerStatus>(out var health))
            {
                health.TakeDamage(2);
            }
        }
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        float dist = Vector3.Distance(transform.position, playerPosition);
        if (dist <= detectRange)
        {
            if (!hasReacted)
            {
                ShowAlertIcon(); // ← ここでも呼べる（使い分けOK）
                hasReacted = true;
            }
            EnterCombat();
        }
        else
        {
            ExitCombat();
            hasReacted = false;
        }
    }

    public override void OnHit()
    {
        OnHit(1);
    }

    public void OnHit(int damage)
    {
        if (isDead) return;

        isDead = true;

        animator.SetTrigger("Die");

        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
}
