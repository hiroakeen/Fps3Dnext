using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class AnimalBase : MonoBehaviour, IAnimalBehavior
{
    protected NavMeshAgent agent;
    protected Animator animator;

    [Header("Wandering")]
    public float wanderRadius = 10f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 4f;
    public float minMoveTime = 3f;
    public float maxMoveTime = 6f;

    protected float timer;
    protected float currentDuration;
    protected bool isWandering;

    [Header("Audio")]
    [SerializeField, HideInInspector]
    protected AudioClip callSound;
    public float minCallInterval = 10f;
    public float maxCallInterval = 20f;

    protected AudioSource audioSource;
    private float callTimer;
    private float nextCallTime;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        StopWandering();
        ResetCallTimer();
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentDuration)
        {
            if (isWandering) StopWandering();
            else StartWandering();
        }

        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
        HandleCallTimer();
    }

    public virtual void StartWandering()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{name} is not on NavMesh yet.");
            return; // NavMesh�ɏ���ĂȂ��Ȃ疳���ɖړI�n�ݒ肵�Ȃ�
        }

        isWandering = true;
        timer = 0f;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        if (NavMesh.SamplePosition(randomPoint, out var hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    public virtual void StopWandering()
    {
        isWandering = false;
        timer = 0f;
        currentDuration = Random.Range(minIdleTime, maxIdleTime);

        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    public abstract void ReactToPlayer(Vector3 playerPosition);

    public virtual void OnHit()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die"); // AnyState �� Death �ɑJ��
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // �����E�����蔻��Ȃǖ�����
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // ���S��ɃI�u�W�F�N�g���폜
        Destroy(gameObject, 2f);

        Debug.Log($"{gameObject.name} was hit and is dying.");
    }
    protected virtual void HandleCallTimer()
    {
        if (!isWandering || callSound == null || audioSource == null) return;

        callTimer += Time.deltaTime;

        if (callTimer >= nextCallTime)
        {
            PlayCallSound();
            ResetCallTimer();
        }
    }

    protected virtual void PlayCallSound()
    {
        audioSource.PlayOneShot(callSound);
    }

    protected virtual void ResetCallTimer()
    {
        callTimer = 0f;
        nextCallTime = Random.Range(minCallInterval, maxCallInterval);
    }

}
