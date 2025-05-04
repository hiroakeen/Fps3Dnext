using UnityEngine;
using System.Collections;

public class Rabbit : AnimalBase, IHittable
{
    [Header("Rabbit Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private AudioClip rabbitCallSound;

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

        // Animator 状態管理（移動中なら鳴き声を繰り返す）
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

        // プレイヤー接近チェック → 自動逃走
        if (player != null && agent != null && agent.isOnNavMesh && !isDead && fleeRoutine == null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < detectPlayerRange)
            {
                if (!hasReacted)
                {
                    ShowAlertIcon(); // ← ビックリマーク表示
                    hasReacted = true;
                }

                StartFleeFromPlayer(player.position);
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
            agent.ResetPath(); // 停止
        }

        fleeRoutine = null;
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (agent == null || isDead) return;

        ShowAlertIcon(); // ← 任意にReactToPlayerでも表示可能

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
