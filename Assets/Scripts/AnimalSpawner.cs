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
    [SerializeField] private AnimalSpawnData[] animalTypes;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    private float timer;
    private GameObject currentTigerInstance = null;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnAnimal();
            timer = 0f;
        }
    }

    void SpawnAnimal()
    {
        if (animalTypes.Length == 0 || spawnPoints.Length == 0) return;

        AnimalSpawnData selectedData = ChooseByWeight();
        if (selectedData == null) return;

        if (selectedData.uniqueSpawn && currentTigerInstance != null) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // NavMesh 上の有効な位置をサンプル
        Vector3 spawnPos = point.position;
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            GameObject instance = Instantiate(selectedData.prefab, hit.position, point.rotation);

           
        }
        else
        {
            Debug.LogWarning("Spawn position not on NavMesh: " + point.name);
        }
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
