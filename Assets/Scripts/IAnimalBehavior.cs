
public interface IAnimalBehavior
{
    void StartWandering();
    void StopWandering();
    void ReactToPlayer(UnityEngine.Vector3 playerPosition);
    void OnHit();
}
