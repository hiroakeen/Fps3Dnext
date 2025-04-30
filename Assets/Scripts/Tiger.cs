
using UnityEngine;

public class Tiger : AnimalBase
{
    public override void ReactToPlayer(Vector3 playerPosition)
    {
        // プレイヤーに向かって近づく例（追跡型）
        agent.SetDestination(playerPosition);
        isWandering = true;
        currentDuration = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void OnHit()
    {
        base.OnHit();
        // ここにトラ特有の死亡処理などを追加可
    }
}
