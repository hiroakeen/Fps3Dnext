using UnityEngine;

public class Rabbit : AnimalBase, IHittable
{
    [SerializeField] private GameObject foodPrefab;

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

        // ÉtÅ[Éhê∂ê¨
        if (foodPrefab != null)
        {
            Instantiate(foodPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        base.OnHit();
    }
}
