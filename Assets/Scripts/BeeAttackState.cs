using UnityEngine;

public class BeeAttackState : IState
{
    private Bee bee;
    private float attackTimer = 0f;
    private float preAttackTimer = 0f;

    public BeeAttackState(Bee bee) => this.bee = bee;

    public void Enter()
    {
        bee.Agent.ResetPath();
        bee.PlayAnimation("Idle");
    }

    public void Update()
    {
        float distance = Vector3.Distance(bee.transform.position, bee.Player.position);

        if (distance > bee.attackRange)
        {
            bee.SetState(new BeeChaseState(bee));
            return;
        }

        Vector3 direction = (bee.Player.position - bee.transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            bee.transform.rotation = Quaternion.RotateTowards(bee.transform.rotation, toRotation, 720f * Time.deltaTime);
        }

        preAttackTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (preAttackTimer >= bee.PreAttackWait && attackTimer >= bee.attackCooldown)
        {
            bee.Animator.SetTrigger("Attack");
            attackTimer = 0f;
            preAttackTimer = 0f;

            if (distance <= bee.attackRange && bee.Player.TryGetComponent<PlayerStatus>(out var health))
            {
                health.TakeDamage(1);
            }
        }
    }

    public void Exit() { }
}