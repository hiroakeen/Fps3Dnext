using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bear : AnimalBase, IDamageable, IReactive
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

    private IState currentState;

    public NavMeshAgent Agent => agent;
    public Animator Animator => animator;
    public Transform Player => player;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public string[] AttackTriggers => attackTriggers;

    private readonly string[] attackTriggers = { "Attack1", "Attack2", "Attack3" };

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        callSound = bearCallSound;
        SetState(new BearIdleState(this));
    }

    protected override void Update()
    {
        if (isDead || player == null || !hasBeenAttacked) return;
        base.Update();
        currentState?.Update();
    }

    public void SetState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (!hasReacted)
        {
            ShowAlertIcon(2.8f);
            hasReacted = true;
        }
        SetState(new BearChaseState(this));
    }

    public void ReactToHit()
    {
        if (!hasBeenAttacked)
        {
            hasBeenAttacked = true;
            SetState(new BearChaseState(this));
            ShowAlertIcon(2.8f);
        }
    }

    public void OnHit(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        ReactToHit();

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

    public void TryAttack()
    {
        if (attackTimer < attackCooldown) return;

        attackTimer = 0f;
        string trigger = attackTriggers[Random.Range(0, attackTriggers.Length)];
        animator.SetTrigger(trigger);

        Invoke(nameof(DealDamageToPlayer), 0.5f);
    }

    public void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 720f * Time.deltaTime);
        }
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