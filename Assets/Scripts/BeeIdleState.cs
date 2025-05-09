using UnityEngine;
public class BeeIdleState : IState
{
    private Bee bee;

    public BeeIdleState(Bee bee) => this.bee = bee;

    public void Enter()
    {
        bee.PlayAnimation("Idle");
        bee.Agent.SetDestination(bee.InitialPosition);
    }

    public void Update()
    {
        float dist = Vector3.Distance(bee.transform.position, bee.Player.position);
        if (dist <= bee.detectionRange)
        {
            bee.ReactToPlayer(bee.Player.position);
            return;
        }

        float homeDist = Vector3.Distance(bee.transform.position, bee.InitialPosition);
        if (homeDist > 0.5f)
        {
            bee.Agent.SetDestination(bee.InitialPosition);
            bee.PlayAnimation("Move");
        }
        else
        {
            bee.Agent.ResetPath();
            bee.PlayAnimation("Idle");
        }

        bee.Animator.SetFloat("Speed", bee.Agent.velocity.magnitude);
    }

    public void Exit() { }
}
