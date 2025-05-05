using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AnimalSpawnData
{
    public GameObject prefab;
    public float spawnWeight = 1f;
    public bool uniqueSpawn = false;
}

public class AnimalSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private AnimalSpawnData[] animalTypes;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float minSpawnDistanceFromPlayer = 20f;
    [SerializeField] private float maxSpawnRadius = 50f;
    [SerializeField] private int maxSampleAttempts = 10;

    [Header("Area Constraint")]
    [SerializeField] private Collider spawnAreaCollider; // トリガー範囲のコライダー

    private float timer = 0f;
    private GameObject currentTigerInstance = null;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found.");
        }
    }

    void Update()
    {
        if (player == null || spawnAreaCollider == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnAnimal();
            timer = 0f;
        }
    }

    void SpawnAnimal()
    {
        AnimalSpawnData selectedData = ChooseByWeight();
        if (selectedData == null) return;

        if (selectedData.uniqueSpawn && currentTigerInstance != null) return;

        Vector3? spawnPos = GetValidSpawnPosition();
        if (spawnPos == null)
        {
            Debug.LogWarning("Valid spawn position not found.");
            return;
        }

        GameObject instance = Instantiate(selectedData.prefab, spawnPos.Value, Quaternion.identity);

        if (selectedData.uniqueSpawn)
        {
            currentTigerInstance = instance;
        }
    }

    Vector3? GetValidSpawnPosition()
    {
        for (int i = 0; i < maxSampleAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistanceFromPlayer, maxSpawnRadius);
            Vector3 candidatePos = player.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                if (spawnAreaCollider.bounds.Contains(hit.position))
                {
                    return hit.position;
                }
            }
        }

        return null;
    }

    AnimalSpawnData ChooseByWeight()
    {
        float totalWeight = 0f;

        foreach (var animal in animalTypes)
        {
            if (animal.uniqueSpawn && currentTigerInstance != null) continue;
            totalWeight += animal.spawnWeight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var animal in animalTypes)
        {
            if (animal.uniqueSpawn && currentTigerInstance != null) continue;

            cumulative += animal.spawnWeight;
            if (randomValue <= cumulative)
                return animal;
        }

        return null;
    }
}
