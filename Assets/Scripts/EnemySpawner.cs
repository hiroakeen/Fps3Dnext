using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private int spawnTime = 5;


    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }


    private IEnumerator SpawnLoop()
    {
        for (int i = 0; i < GameManager.Instance.totalEnemies ; i++)
        {
            var distanceVector = new Vector3(10, 0);
            var spawnPositionFromPlayer = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0) * distanceVector;
            var spawnPosition = player.transform.position + spawnPositionFromPlayer;

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(spawnPosition, out navMeshHit, 10, NavMesh.AllAreas))
            {
                Instantiate(enemyPrefab[UnityEngine.Random.Range(0, 4)], navMeshHit.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnTime);

            }
        }
    }
}


