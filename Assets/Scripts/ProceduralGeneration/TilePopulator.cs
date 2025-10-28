using System.Collections.Generic;
using UnityEngine;

public class TilePopulator : MonoBehaviour
{

    [Header("Tile Objects")]
    public GameObject tile;
    public Transform[] availableSpawns;
    public GameObject[] availableObjects;
    public GameObject[] availableEnemies;

    [Header("Tile Settings")]
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

        // Spawn new ones
        foreach (Transform spawn in availableSpawns)
        {
            //int randomEnemySpawnPercentage = Random.Range(0, 100);
            int randomIndex = Random.Range(0, availableObjects.Length);
            GameObject prefab = availableObjects[randomIndex];
            GameObject spawnedItem = Instantiate(prefab, spawn.position, spawn.rotation, spawn);
            spawnedObjects.Add(spawnedItem);
        }
    }
}
