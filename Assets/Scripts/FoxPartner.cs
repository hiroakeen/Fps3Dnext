using UnityEngine;
using UnityEngine.AI;

public class FoxPartner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private float followDistance = 5f;
    [SerializeField] private float detectionRadius = 30f;
    [SerializeField] private float orbitDistance = 3f;
    [SerializeField] private float orbitSpeed = 2f;
    [SerializeField] private float runThreshold = 15f;
    [SerializeField] private float walkThreshold = 7f;
    [SerializeField] private float maxGuideDistance = 20f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform nearestAnimal;
    private float orbitAngle = 0f;
    private float idleTimer = 0f;
    private bool hasStartedGuiding = false;

    private enum FoxState { Idle, Walk, Run }
    private FoxState currentState = FoxState.Idle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        nearestAnimal = FindNearestAnimal();

        if (!hasStartedGuiding && nearestAnimal != null)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2f)
            {
                hasStartedGuiding = true;
            }
            else
            {
                SetState(FoxState.Idle);
                return;
            }
        }

        if (nearestAnimal != null && Vector3.Distance(transform.position, nearestAnimal.position) < orbitDistance + 1f)
        {
            OrbitAroundAnimal();
        }
        else if (hasStartedGuiding && nearestAnimal != null)
        {
            GuideToAnimal();
        }
        else
        {
            OrbitAroundPlayer();
        }
    }

    private void GuideToAnimal()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxGuideDistance)
        {
            FollowPlayerIdle();
            return;
        }

        if (nearestAnimal != null)
        {
            Vector3 targetPos = Vector3.Lerp(player.position, nearestAnimal.position, 0.7f);
            float distanceToAnimal = Vector3.Distance(transform.position, nearestAnimal.position);

            if (distanceToAnimal > runThreshold)
            {
                SetState(FoxState.Run);
                agent.speed = 6f;
            }
            else if (distanceToAnimal > walkThreshold)
            {
                SetState(FoxState.Walk);
                agent.speed = 3f;
            }
            else
            {
                SetState(FoxState.Idle);
                agent.ResetPath();
                return;
            }

            agent.SetDestination(targetPos);
        }
    }

    private void FollowPlayerIdle()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            agent.SetDestination(player.position);
            SetState(FoxState.Walk);
        }
        else
        {
            agent.ResetPath();
            SetState(FoxState.Idle);
        }
    }

    private void OrbitAroundAnimal()
    {
        orbitAngle += orbitSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(orbitAngle), 0, Mathf.Sin(orbitAngle)) * orbitDistance;
        Vector3 orbitTarget = nearestAnimal.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(orbitTarget, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            SetState(FoxState.Walk);
        }
    }

    private void OrbitAroundPlayer()
    {
        orbitAngle += orbitSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(orbitAngle), 0, Mathf.Sin(orbitAngle)) * followDistance;
        Vector3 orbitTarget = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(orbitTarget, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            SetState(FoxState.Walk);
        }
    }

    private Transform FindNearestAnimal()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        Transform closest = null;
        float minDistance = detectionRadius;

        foreach (GameObject animal in animals)
        {
            float dist = Vector3.Distance(transform.position, animal.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = animal.transform;
            }
        }
        return closest;
    }

    private void SetState(FoxState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case FoxState.Idle:
                animator.SetTrigger("Idle");
                break;
            case FoxState.Walk:
                animator.SetTrigger("Walk");
                break;
            case FoxState.Run:
                animator.SetTrigger("Run");
                break;
        }
    }
}