using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator armAnimator;
    [SerializeField] private Animator shadowAnimator;

    public void SetMoveSpeed(float value)
    {
        if (armAnimator != null) armAnimator.SetFloat("MoveSpeed", value);
        if (shadowAnimator != null) shadowAnimator.SetFloat("MoveSpeed", value);
    }

    public void SetAttackState(bool isAttacking)
    {
        armAnimator?.SetBool("Attack", isAttacking);
        shadowAnimator?.SetBool("Attack", isAttacking);
    }

    public void TriggerArrowAttack()
    {
        armAnimator?.SetTrigger("AttackArrow");
        shadowAnimator?.SetTrigger("AttackArrow");
    }
}
