using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject entrancePrefab;
    public GameObject[] tilePrefabs; // assign various tile prefabs
    public int numberOfTiles = 7;
    public int tileScale = 5;

    public List<Vector3> usedPositions;

    private void Start()
    {
        CreateDungeon();
    }

    private void Update()
    {
        
    }


    public void CreateDungeon()
    {
        Vector3 currentPosition = Vector3.zero;
        usedPositions.Add(currentPosition);

        // Create entrance
        Instantiate(entrancePrefab, currentPosition, Quaternion.identity);

        for (int i = 0; i < numberOfTiles; i++)
        {
            Vector3 newPosition = GetNewPosition(currentPosition);

            // Avoid overlapping tiles
            int safety = 0;
            while (usedPositions.Contains(newPosition) && safety < 10)
            {
                newPosition = GetNewPosition(currentPosition);
                safety++;
            }

            GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            GameObject tile = Instantiate(prefab, newPosition, Quaternion.identity);
            usedPositions.Add(newPosition);
            currentPosition = newPosition;
        }
    }

    private Vector3 GetNewPosition(Vector3 current)
    {
        // Choose random direction (up, down, left, right)
        Vector3[] directions = new Vector3[]
        {
        new Vector3(tileScale, 0, 0),   // right
        new Vector3(-tileScale, 0, 0),  // left
        new Vector3(0, tileScale, 0),   // up
        new Vector3(0, -tileScale, 0),  // down
        };

        return current + directions[Random.Range(0, directions.Length)];
    }
}
