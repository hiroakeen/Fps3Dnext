using UnityEngine;
using UnityEngine.AI;

public class Bear : AnimalBase, IDamageable
{
    [Header("Bear Settings")]
    public float detectRange = 15f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    [SerializeField] private AudioClip bearCallSound;
    [SerializeField] private GameObject foodPrefab;

    private Transform player;
    private bool isDead = false;
    private bool hasReacted = false;
    private bool hasBeenAttacked = false;

    private float attackTimer = 0f;
    private int currentHealth = 3;

    private enum BearState { Idle, Chase, Combat }
    private BearState currentState = BearState.Idle;

    private readonly string[] attackTriggers = { "Attack1", "Attack2", "Attack3" };

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        callSound = bearCallSound;
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || player == null || !hasBeenAttacked) return; // 攻撃されるまで何もしない

        float dist = Vector3.Distance(transform.position, player.position);
        attackTimer += Time.deltaTime;

        switch (currentState)
        {
            case BearState.Idle:
                SetSpeedParameter(0f);
                if (dist <= attackRange)
                {
                    SetState(BearState.Combat);
                }
                break;

            case BearState.Chase:
                if (dist > attackRange)
                {
                    if (!agent.hasPath || agent.destination != player.position)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(player.position);
                    }
                    SetSpeedParameter(agent.velocity.magnitude);
                }
                else
                {
                    SetState(BearState.Combat);
                }
                break;

            case BearState.Combat:
                agent.isStopped = true;
                SetSpeedParameter(0f);

                if (dist > attackRange)
                {
                    SetState(BearState.Chase);
                }
                else
                {
                    FacePlayer();
                    TryAttack();
                }
                break;
        }
    }
    private void SetState(BearState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        animator.SetBool("CombatIdle", newState == BearState.Combat);
    }

    private void SetSpeedParameter(float velocity)
    {
        float speedValue = Mathf.InverseLerp(0f, 3f, velocity) * 2f;
        animator.SetFloat("Speed", speedValue);
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 720f * Time.deltaTime);
        }
    }

    private void TryAttack()
    {
        if (attackTimer < attackCooldown) return;

        attackTimer = 0f;
        string trigger = attackTriggers[Random.Range(0, attackTriggers.Length)];
        animator.SetTrigger(trigger);

        // 遅延攻撃（0.5秒後にダメージ適用）
        Invoke(nameof(DealDamageToPlayer), 0.5f);
    }


    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (!hasReacted)
        {
            ShowAlertIcon(2.8f);
            hasReacted = true;
        }
        SetState(BearState.Chase);
    }

    public override void OnHit()
    {
        OnHit(1);
    }

    public void OnHit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (!hasBeenAttacked)
        {
            hasBeenAttacked = true;
            SetState(BearState.Chase); // 初攻撃時に追跡開始
            ShowAlertIcon(2.8f); // 初反応
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
    private void DealDamageToPlayer()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.TryGetComponent<PlayerStatus>(out var health))
            {
                health.TakeDamage(3);
            }
        }
    }

}
