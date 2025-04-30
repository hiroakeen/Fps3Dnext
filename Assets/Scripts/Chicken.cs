
using UnityEngine;

public class Chicken : AnimalBase
{
    public override void ReactToPlayer(Vector3 playerPosition)
    {
        Vector3 dir = (transform.position - playerPosition).normalized;
        agent.SetDestination(transform.position + dir * wanderRadius);
        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }
}

