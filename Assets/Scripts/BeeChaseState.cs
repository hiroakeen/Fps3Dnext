using UnityEngine;

public class BeeChaseState : IState
{
    private Bee bee;

    public BeeChaseState(Bee bee) => this.bee = bee;

    public void Enter() => bee.PlayAnimation("Move");

    public void Update()
    {
        float distance = Vector3.Distance(bee.transform.position, bee.Player.position);

        if (distance > bee.maxChaseDistance)
        {
            bee.SetState(new BeeIdleState(bee));
            return;
        }

        if (distance <= bee.attackRange)
        {
            bee.SetState(new BeeAttackState(bee));
            return;
        }

        bee.Agent.SetDestination(bee.Player.position);
        bee.Animator.SetFloat("Speed", bee.Agent.velocity.magnitude);
    }

    public void Exit() => bee.Agent.ResetPath();
}
