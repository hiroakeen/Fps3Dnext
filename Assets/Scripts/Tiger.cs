
using UnityEngine;

public class Tiger : AnimalBase
{
    public override void ReactToPlayer(Vector3 playerPosition)
    {
        // �v���C���[�Ɍ������ċ߂Â���i�ǐՌ^�j
        agent.SetDestination(playerPosition);
        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void OnHit()
    {
        base.OnHit();
        // �����Ƀg�����L�̎��S�����Ȃǂ�ǉ���
    }
}
