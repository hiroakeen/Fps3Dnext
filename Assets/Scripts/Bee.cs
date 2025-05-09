using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bee : AnimalBase, IDamageable
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

    private IState currentState;

    public NavMeshAgent Agent => agent;
    public Animator Animator => animator;
    public Transform Player => player;
    public Vector3 InitialPosition => initialPosition;
    public float PreAttackWait => 0.5f;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

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

        SetState(new BeeIdleState(this));
    }

    protected override void Update()
    {
        if (isDead || player == null) return;
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
        ShowAlertIcon(1.5f);
        SetState(new BeeChaseState(this));
    }

    public override void OnHit()
    {
        Die();
    }

    public void OnHit(int damage)
    {
        if (isDead) return;
        Die();
    }

    private void Die()
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
        Destroy(gameObject, 1.2f);
    }
}

public static class BeeExtensions
{
    public static void PlayAnimation(this Bee bee, string name)
    {
        if (!bee.Animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            bee.Animator.Play(name);
        }
    }
}