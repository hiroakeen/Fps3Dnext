using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Rabbit : AnimalBase, IHittable
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private AudioClip rabbitCallSound;
    private Coroutine callRoutine;
    private bool isMoving = false;

    protected override void Start()
    {
        base.Start();
        callSound = rabbitCallSound;
    }
    protected override void Update()
    {
        if (isDead) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool nowMoving = state.IsName("Move"); // ← Animatorのステート名 "Move" に一致

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
    }

    public override void ReactToPlayer(Vector3 playerPosition)
    {
        if (agent == null || isDead) return;

        Vector3 dir = (transform.position - playerPosition).normalized;
        float fleeDistance = wanderRadius * 1.5f;
        agent.speed = 5.5f;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(transform.position + dir * fleeDistance);
        }

        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }

    private bool isDead = false;

    public override void OnHit()
    {
        if (isDead) return;
        isDead = true;

        // フード生成
        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        base.OnHit();
    }
    private IEnumerator CallRepeatedly(int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            if (callSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(callSound);
            }
            yield return new WaitForSeconds(interval);
        }
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
