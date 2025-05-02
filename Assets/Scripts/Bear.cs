using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour,IDamageable
{
    public float detectRange = 15f;
    public float attackRange = 3f;
    public float sitChance = 0.05f;
    public float sitDuration = 10f;

    private int hitCount = 0;
    private bool isDead = false;
    private bool isSitting = false;
    private bool inCombat = false;

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;
    private float sitTimer;
    private float randomCheckTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        if (dist <= detectRange)
        {
            EnterCombat();
        }
        else
        {
            ExitCombat();
            if (!isSitting)
            {
                RandomBehavior();
            }
        }

        // Sit中の時間管理
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
            else
            {
                Wander();
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

    void Wander()
    {
        Vector3 point = transform.position + Random.insideUnitSphere * 6f;
        if (NavMesh.SamplePosition(point, out var hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void EnterCombat()
    {
        if (isSitting) return; // 座ってたら無視
        if (inCombat) return;

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
            int index = Random.Range(1, 4);
            animator.SetTrigger($"Attack{index}");

            if (player.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(2); // クマは即死（2ダメージ）
            }
        }
    }


    public void OnHit() => OnHit(1);

    public void OnHit(int damage)
    {
        if (isDead) return;

        hitCount += damage;
        Debug.Log($"Bear took {damage} damage (total: {hitCount})");

        if (hitCount >= 3)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        GetComponent<Animator>()?.SetTrigger("Die");
        if (TryGetComponent<NavMeshAgent>(out var agent))
        {
            agent.isStopped = true;
        }

        Destroy(gameObject, 3f);
    }
}
