using UnityEngine;

public class BearChaseState : IState
{
    private Bear bear;

    public BearChaseState(Bear bear) => this.bear = bear;

    public void Enter()
    {
        bear.Animator.SetBool("CombatIdle", false);
    }

    public void Update()
    {
        float dist = Vector3.Distance(bear.transform.position, bear.Player.position);

        if (dist > bear.AttackRange)
        {
            if (!bear.Agent.hasPath || bear.Agent.destination != bear.Player.position)
            {
                bear.Agent.isStopped = false;
                bear.Agent.SetDestination(bear.Player.position);
            }
            bear.Animator.SetFloat("Speed", bear.Agent.velocity.magnitude);
        }
        else
        {
            bear.SetState(new BearCombatState(bear));
        }
    }

    public void Exit() => bear.Agent.ResetPath();
}