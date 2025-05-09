public interface IAnimalBehavior : IHittable
{
    void StartWandering();
    void StopWandering();
    void ReactToPlayer(UnityEngine.Vector3 playerPosition);
    void OnHit();

}
