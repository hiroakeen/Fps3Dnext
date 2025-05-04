using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bee : MonoBehaviour, IHittable
{
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float moveRadius = 5f;
    public float moveInterval = 3f;
    public float attackCooldown = 1.5f;

    [Header("Bee Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private AudioClip spawnSound;

    [Header("Alert Settings")]
    [SerializeField] private GameObject alertIconPrefab;
    [SerializeField] private Transform alertAnchor;
    [SerializeField] private float alertDisplayTime = 1.5f;

    private GameObject currentAlertIcon;
    private bool hasReacted = false;

    private AudioSource audioSource;
    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;

    private float moveTimer;
    private float attackTimer;
    private bool isDead = false;

    private Vector3 initialPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        audioSource = GetComponent<AudioSource>();
        if (spawnSound != null && audioSource != null)
        {
            audioSource.clip = spawnSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        initialPosition = transform.position;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        moveTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (distance <= attackRange)
        {
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }
        }
        else if (distance <= detectionRange)
        {
            if (!hasReacted)
            {
                ShowAlertIcon();
                hasReacted = true;
            }

            ChasePlayer();
        }
        else
        {
            hasReacted = false;
            ReturnToOrigin();
        }

        // プレイヤー方向に回転（Y軸のみ）
        if (distance <= detectionRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.Play("Move");
    }

    void ReturnToOrigin()
    {
        float distanceToOrigin = Vector3.Distance(transform.position, initialPosition);

        if (distanceToOrigin > 0.5f)
        {
            agent.SetDestination(initialPosition);
            agent.speed = 2.5f;
            animator.Play("Move");
        }
        else
        {
            agent.ResetPath();
            animator.Play("Idle");
        }
    }

    void Attack()
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

    public void OnHit()
    {
        if (isDead) return;

        isDead = true;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // 死亡時に羽音停止
        }

        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    private void ShowAlertIcon()
    {
        if (alertIconPrefab == null || alertAnchor == null) return;

        if (currentAlertIcon != null)
        {
            Destroy(currentAlertIcon);
        }

        currentAlertIcon = Instantiate(alertIconPrefab, alertAnchor.position, Quaternion.identity, alertAnchor);
        Destroy(currentAlertIcon, alertDisplayTime);
    }
}
