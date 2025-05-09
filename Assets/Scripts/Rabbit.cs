using UnityEngine;
using System.Collections;

public class Rabbit : AnimalBase, IDamageable
{
    [Header("Rabbit Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private AudioClip rabbitCallSound;  //ñ¬Ç´ê∫Ç≈ÇÕÇ»Ç≠ë´âπ

    [Header("Flee Settings")]
    [SerializeField] private float detectPlayerRange = 7f;
    [SerializeField] private float fleeSpeed = 5.5f;
    [SerializeField] private float fleeDuration = 3f;

    private Coroutine callRoutine;
    private Coroutine fleeRoutine;
    private bool isMoving = false;
    private bool isDead = false;
    private bool hasReacted = false;
    private Transform player;

    protected override void Start()
    {
        base.Start();
        callSound = rabbitCallSound;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool nowMoving = state.IsName("Move");

        if (nowMoving && !isMoving)
        {
            isMoving = true;
            callRoutine = StartCoroutine(CallWhileMoving(1f));
        }
        else if (!nowMoving && isMoving)
        {
            isMoving = false;
            if (callRoutine != null)
            {
                StopCoroutine(callRoutine);
                callRoutine = null;
            }
        }

        if (player != null && agent != null && agent.isOnNavMesh && !isDead && fleeRoutine == null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < detectPlayerRange)
            {
                if (!hasReacted)
                {
                    ReactToPlayer(player.position);
                    hasReacted = true;
                }
            }
            else
            {
                hasReacted = false;
            }
        }

    }

    private void StartFleeFromPlayer(Vector3 playerPosition)
    {
        Vector3 fleeDir = (transform.position - playerPosition).normalized;
        Vector3 fleeTarget = transform.position + fleeDir * 10f;

        if (agent.isOnNavMesh)
        {
            agent.speed = fleeSpeed;
            agent.SetDestination(fleeTarget);
            if (fleeRoutine != null) StopCoroutine(fleeRoutine);
            fleeRoutine = StartCoroutine(FleeTimer(fleeDuration));
        }
    }

    private IEnumerator FleeTimer(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }

        fleeRoutine = null;
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (agent == null || isDead) return;

        ShowAlertIcon(0.5f);

        Vector3 dir = (transform.position - playerPosition).normalized;
        float fleeDistance = wanderRadius * 1.5f;
        agent.speed = fleeSpeed;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(transform.position + dir * fleeDistance);
        }

        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void OnHit()
    {
        if (isDead) return;
        Die();
    }

    public void OnHit(int damage)
    {
        if (isDead) return;
        Die();
    }

    private void Die()
    {
        isDead = true;

        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        base.OnHit();
    }

    private IEnumerator CallWhileMoving(float interval)
    {
        while (true)
        {
            if (callSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(callSound);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
