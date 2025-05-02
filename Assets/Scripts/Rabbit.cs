using UnityEngine;

public class Rabbit : AnimalBase, IHittable
{
    public override void ReactToPlayer(Vector3 playerPosition)
    {
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

    public override void OnHit()
    {
        Destroy(gameObject); // ‘¦Ž€
    }

}
