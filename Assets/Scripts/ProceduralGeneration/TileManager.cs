using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject entrancePrefab;
    public GameObject[] tilePrefabs; // Assign various tile prefabs
    public int numberOfTiles = 7;
    public int tileScale = 5;

    public List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        Vector3 currentPosition = Vector3.zero;
        usedPositions.Add(currentPosition);

        // Create entrance
        GameObject entrance = Instantiate(entrancePrefab, currentPosition, Quaternion.identity);
        Tile currentTile = entrance.GetComponent<Tile>();

        for (int i = 0; i < numberOfTiles; i++)
        {
            // Get available spawn directions from current tile
            List<Vector3> availableDirs = GetAvailableDirections(currentTile, currentPosition);

            if (availableDirs.Count == 0)
            {
                Debug.Log("No available exits from current tile, stopping generation.");
                break;
            }

            // Pick a random direction from available ones
            Vector3 chosenDir = availableDirs[Random.Range(0, availableDirs.Count)];
            Vector3 newPosition = currentPosition + chosenDir * tileScale;

            // Avoid overlapping tiles
            int safety = 0;
            while (usedPositions.Contains(newPosition) && safety < 10)
            {
                chosenDir = availableDirs[Random.Range(0, availableDirs.Count)];
                newPosition = currentPosition + chosenDir * tileScale;
                safety++;
            }

            // Pick a random tile that fits (must have the matching opposite exit)
            GameObject prefab = GetMatchingTile(chosenDir);
            if (prefab == null)
            {
                Debug.LogWarning("No matching tile found for direction " + chosenDir);
                break;
            }

            // Spawn tile
            GameObject newTileObj = Instantiate(prefab, newPosition, Quaternion.identity);
            Tile newTile = newTileObj.GetComponent<Tile>();

            usedPositions.Add(newPosition);
            currentPosition = newPosition;
            currentTile = newTile;
        }
    }

    private List<Vector3> GetAvailableDirections(Tile tile, Vector3 currentPos)
    {
        List<Vector3> dirs = new List<Vector3>();

        if (tile.exitRight)
            dirs.Add(Vector3.right);
        if (tile.exitLeft)
            dirs.Add(Vector3.left);
        if (tile.exitTop)
            dirs.Add(Vector3.up);
        if (tile.exitBottom)
            dirs.Add(Vector3.down);

        return dirs;
    }

    private GameObject GetMatchingTile(Vector3 direction)
    {
        List<GameObject> candidates = new List<GameObject>();

        foreach (var prefab in tilePrefabs)
        {
            Tile t = prefab.GetComponent<Tile>();
            if (t == null) continue;

            if (direction == Vector3.right && t.exitLeft) candidates.Add(prefab);
            if (direction == Vector3.left && t.exitRight) candidates.Add(prefab);
            if (direction == Vector3.up && t.exitBottom) candidates.Add(prefab);
            if (direction == Vector3.down && t.exitTop) candidates.Add(prefab);
        }

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }
}
