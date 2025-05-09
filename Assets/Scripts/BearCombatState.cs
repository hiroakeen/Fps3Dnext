using UnityEngine;

public class BearCombatState : IState
{
    private Bear bear;

    public BearCombatState(Bear bear) => this.bear = bear;

    public void Enter()
    {
        bear.Agent.isStopped = true;
        bear.Animator.SetBool("CombatIdle", true);
        bear.Animator.SetFloat("Speed", 0f);
    }

    public void Update()
    {
        float dist = Vector3.Distance(bear.transform.position, bear.Player.position);

        if (dist > bear.AttackRange)
        {
            bear.SetState(new BearChaseState(bear));
        }
        else
        {
            bear.FacePlayer();
            bear.TryAttack();
        }
    }

    public void Exit() { }
}