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

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        StopWandering();
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
    }

    public virtual void StartWandering()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{name} is not on NavMesh yet.");
            return; // NavMeshに乗ってないなら無理に目的地設定しない
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
        Debug.Log($"{gameObject.name} was hit!");
        // 任意で死亡アニメーションやラグドール化など
    }
}
