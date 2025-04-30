
using UnityEngine;

public class Deer : AnimalBase
{
    public override void ReactToPlayer(Vector3 playerPosition)
    {
        Vector3 dir = (transform.position - playerPosition).normalized;
        agent.SetDestination(transform.position + dir * (wanderRadius * 1.5f));
        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }
}