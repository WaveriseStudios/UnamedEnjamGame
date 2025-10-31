using System.Collections.Generic;
using UnityEngine;

public class TilePopulator : MonoBehaviour
{

    [Header("Tile Objects")]
    public GameObject[] availableObjects;
    public GameObject[] availableEnemies;

    [SerializeField] private Vector2 areaSize = new Vector2(10, 10);  // Width & height
    [SerializeField] private Transform center;    // Center of the square (optional)

    [Header("Tile Settings")]
    public int populatePercentage = 75;
    public int enemyPercentage = 33;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        InstantiateObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InstantiateObjects();
        }
    }

    public void InstantiateObjects()
    {
        // Clear previously spawned objects safely
        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                Destroy(item);
        }
        spawnedObjects.Clear();

        int randomSpawnPercentage = Random.Range(0, 100);
        if (randomSpawnPercentage < populatePercentage)
        {
            int randomEnemyPercentage = Random.Range(0, 100);

            if(randomEnemyPercentage < enemyPercentage)
            {
                for (int i = 0; i < availableEnemies.Length; i++)
                {
                    {
                        Vector2 pos = GetRandomPosition();

                        int randomIndex = Random.Range(0, availableEnemies.Length);
                        GameObject prefab = availableEnemies[randomIndex];
                        GameObject spawnedEnemy = Instantiate(prefab, pos, Quaternion.identity);
                        spawnedEnemy.GetComponent<Zombie>().playerPos = FindFirstObjectByType<PlayerCharacter>().transform;
                        spawnedObjects.Add(spawnedEnemy);
                    }
                }
            }
            else
            {
                for (int i = 0; i < availableObjects.Length; i++)
                {
                    {
                        Vector2 pos = GetRandomPosition();

                        int randomIndex = Random.Range(0, availableObjects.Length);
                        GameObject prefab = availableObjects[randomIndex];
                        GameObject spawnedItem = Instantiate(prefab, pos, Quaternion.identity);
                        spawnedObjects.Add(spawnedItem);
                    }
                }
            }
        }

        // Spawn new ones

        Vector2 GetRandomPosition()
        {
            Vector2 halfSize = areaSize / 2f;
            Vector2 randomOffset = new Vector2(
                Random.Range(-halfSize.x, halfSize.x),
                Random.Range(-halfSize.y, halfSize.y)
            );

            // If you have a center Transform, offset by it
            Vector2 spawnPos = (center != null ? (Vector2)center.position : Vector2.zero) + randomOffset;

            return spawnPos;
        }
    }
}
