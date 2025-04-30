using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] NPCPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private int spawnTime = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            var distanceVector = new Vector3(10, 0);
            var spawnPositionFromPlayer = Quaternion.Euler(0, Random.Range(0, 360f), 0) * distanceVector;
            var spawnPosition = player.transform.position + spawnPositionFromPlayer;

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(spawnPosition, out navMeshHit, 10, NavMesh.AllAreas))
            {
                Instantiate(NPCPrefab[Random.Range(0,4)], navMeshHit.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnTime);
               
            }
        }
    }
}
