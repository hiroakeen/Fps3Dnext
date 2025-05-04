using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bee : AnimalBase, IHittable
{
    public float detectionRange = 8f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public float maxChaseDistance = 15f;

    [Header("Bee Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private AudioClip spawnSound;

    private Transform player;
    private Vector3 initialPosition;
    private bool isDead = false;
    private bool hasReacted = false;
    private float attackTimer = 0f;
    private float preAttackWait = 0.5f; // 攻撃前待機時間
    private float preAttackTimer = 0f;


    private enum BeeState { Idle, Chase, Attack }
    private BeeState currentState = BeeState.Idle;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent.stoppingDistance = attackRange;
        initialPosition = transform.position;

        if (spawnSound != null && audioSource != null)
        {
            audioSource.clip = spawnSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    protected override void Update()
    {
        if (isDead || player == null) return;
        base.Update();

        float distance = Vector3.Distance(transform.position, player.position);
        attackTimer += Time.deltaTime;

        switch (currentState)
        {
            case BeeState.Idle:
                PlayAnimation("Idle");
                ReturnToOrigin();

                if (distance <= detectionRange)
                {
                    ReactToPlayer(player.position);
                }
                break;

            case BeeState.Chase:
                if (distance > maxChaseDistance)
                {
                    SetState(BeeState.Idle);
                    hasReacted = false;
                    return;
                }

                if (distance <= attackRange)
                {
                    SetState(BeeState.Attack);
                }
                else
                {
                    agent.SetDestination(player.position);
                    PlayAnimation("Move");
                }
                break;

            case BeeState.Attack:
                if (distance > attackRange)
                {
                    SetState(BeeState.Chase);
                    preAttackTimer = 0f; // ← リセット
                }
                else
                {
                    // プレイヤーの方向を向く
                    Vector3 direction = (player.position - transform.position).normalized;
                    direction.y = 0f;
                    if (direction != Vector3.zero)
                    {
                        Quaternion toRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
                    }

                    preAttackTimer += Time.deltaTime;

                    // 攻撃待機時間を超えたら攻撃実行
                    if (preAttackTimer >= preAttackWait && attackTimer >= attackCooldown)
                    {
                        Attack();
                        attackTimer = 0f;
                        preAttackTimer = 0f;
                    }
                }
                break;

        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void SetState(BeeState newState)
    {
        currentState = newState;
    }

    private void ReturnToOrigin()
    {
        float dist = Vector3.Distance(transform.position, initialPosition);
        if (dist > 0.5f)
        {
            agent.SetDestination(initialPosition);
            PlayAnimation("Move");
        }
        else
        {
            agent.ResetPath();
            PlayAnimation("Idle");
        }
    }

    private void PlayAnimation(string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            animator.Play(name);
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger("Attack");

        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (player.TryGetComponent<PlayerStatus>(out var health))
            {
                health.TakeDamage(1);
            }
        }
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (!hasReacted)
        {
            ShowAlertIcon(1.5f);
            hasReacted = true;
        }

        SetState(BeeState.Chase);
    }

    public override void OnHit()
    {
        if (isDead) return;

        isDead = true;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        animator.SetTrigger("Die");
        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
}
