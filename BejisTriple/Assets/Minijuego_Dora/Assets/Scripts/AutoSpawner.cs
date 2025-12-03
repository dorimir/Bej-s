using UnityEngine;

public class AutoSpawnerXY : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaculoPrefab;
    public GameObject potenciadorPrefab;

    [Header("Spawn Settings")]
    public float mapLength = 500f;           // Length of the map along X-axis
    public float minSpacing = 10f;           // Min distance between spawns
    public float maxSpacing = 25f;           // Max distance between spawns

    [Header("Spawn Area (Y only)")]
    public float minY = -2f;
    public float maxY = 5f;

    [Header("Z Position (constant)")]
    public float zPos = 0f;

    [Header("Probabilities (0–1)")]
    public float spawnPotenciadorChance = 0.2f;   // 20%
    public float spawnObstaculoChance = 0.5f;     // 50%

    void Start()
    {
        SpawnAlongMap();
    }

    void SpawnAlongMap()
    {
        float x = 0f;

        while (x < mapLength)
        {
            x += Random.Range(minSpacing, maxSpacing);

            float roll = Random.value;

            if (roll < spawnPotenciadorChance)
                SpawnObject(potenciadorPrefab, x);
            else if (roll < spawnPotenciadorChance + spawnObstaculoChance)
                SpawnObject(obstaculoPrefab, x);
        }
    }

    void SpawnObject(GameObject prefab, float xPos)
    {
        if (prefab == null) return;

        Vector3 pos = new Vector3(
            xPos,
            Random.Range(minY, maxY),
            zPos
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}