using UnityEngine;

public class BearIdleState : IState
{
    private Bear bear;

    public BearIdleState(Bear bear) => this.bear = bear;

    public void Enter()
    {
        bear.Animator.SetBool("CombatIdle", false);
        bear.Agent.ResetPath();
        bear.Animator.SetFloat("Speed", 0f);
    }

    public void Update()
    {
        float dist = Vector3.Distance(bear.transform.position, bear.Player.position);
        if (dist <= bear.AttackRange)
        {
            bear.SetState(new BearCombatState(bear));
        }
    }

    public void Exit() { }
}