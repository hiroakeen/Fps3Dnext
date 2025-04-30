using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WanderingAnimal : MonoBehaviour
{
    [Header("Wandering Settings")]
    public float wanderRadius = 10f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 4f;
    public float minMoveTime = 3f;
    public float maxMoveTime = 6f;

    private NavMeshAgent agent;
    private Animator animator;
    private float timer = 0f;
    private float currentDuration = 0f;
    private bool isWalking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetIdle(); // 最初は停止状態
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentDuration)
        {
            if (isWalking)
            {
                SetIdle();
            }
            else
            {
                SetNewDestination();
            }
        }

        // Animator用ブール（歩いてるかどうか）
        if (animator != null)
        {
            animator.SetBool("Move", agent.velocity.magnitude > 0.1f);
        }
    }

    void SetIdle()
    {
        isWalking = false;
        timer = 0f;
        currentDuration = Random.Range(minIdleTime, maxIdleTime);
        agent.ResetPath(); // 停止
    }

    void SetNewDestination()
    {
        isWalking = true;
        timer = 0f;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
