using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileCodeMapping
{
    public string code;       // e.g., "TD", "L", "."
    public GameObject prefab; // prefab to spawn for this code
}

[System.Serializable]
public class TileRow
{
    public List<string> row = new List<string>();
}

public class TileManager : MonoBehaviour
{
    [Header("Tile Settings")]
    public TextAsset levelFile; // txt
    public float tileScale = 5f; // space between tiles
    public List<TileRow> gridRows = new List<TileRow>();
    public TileCodeMapping[] tileMappings;

    [Header("Pad Settings")]
    public GameObject padPrefab;           
    public GameObject endObject;           
    public float minTileDistance = 5f;     
    private List<Vector2> padPositions = new List<Vector2>();
    private int padsActivated = 0;


    private Dictionary<string, GameObject> codeToPrefab;

    private void Start()
    {
        BuildDictionary();

        // First populate the grid
        PopulateGridFromText(levelFile);

        // Then generate the level
        GenerateFromText();
    }


    private void BuildDictionary()
    {
        codeToPrefab = new Dictionary<string, GameObject>();
        foreach (var mapping in tileMappings)
        {
            if (!codeToPrefab.ContainsKey(mapping.code))
                codeToPrefab.Add(mapping.code, mapping.prefab);
        }
    }

    private void GenerateFromText()
    {
        if (levelFile == null)
        {
            Debug.LogError("No level text file assigned!");
            return;
        }

        string[] lines = levelFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        int height = lines.Length;
        if (height == 0) return;
        int width = lines[0].Split(' ').Length;

        // 1️⃣ Find entrance position in the text
        Vector2Int entranceGrid = Vector2Int.zero;
        bool foundEntrance = false;

        for (int y = 0; y < height; y++)
        {
            string[] codes = lines[y].Trim().Split(' ');
            for (int x = 0; x < codes.Length; x++)
            {
                if (codes[x].Trim() == "S") // or your entrance code
                {
                    entranceGrid = new Vector2Int(x, y);
                    foundEntrance = true;
                    break;
                }
            }
            if (foundEntrance) break;
        }

        if (!foundEntrance)
        {
            Debug.LogWarning("No entrance tile found! Defaulting to center.");
            entranceGrid = new Vector2Int(width / 2, height / 2);
        }

        // 2️⃣ Calculate offset to world position (20,20)
        Vector3 entranceWorldPos = new Vector3(0, 10, 0f);
        Vector3 entranceTilePos = new Vector3(entranceGrid.x * tileScale, (height - 1 - entranceGrid.y) * tileScale, 0f);
        Vector3 offset = entranceWorldPos - entranceTilePos;

        // 3️⃣ Spawn all tiles relative to entrance
        for (int y = 0; y < height; y++)
        {
            string[] codes = lines[y].Trim().Split(' ');

            for (int x = 0; x < codes.Length; x++)
            {
                string code = codes[x].Trim();
                if (code == "." || !codeToPrefab.ContainsKey(code))
                    continue; // skip empty

                GameObject prefab = codeToPrefab[code];
                Vector3 pos = new Vector3(x * tileScale, (height - 1 - y) * tileScale, 0f) + offset;
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);

                if (obj.GetComponent<EndTile>())
                {
                    endObject = obj;
                }
            }
        }

        SpawnPads(3);
    }

    private void SpawnPads(int count)
    {
        if (padPrefab == null)
        {
            Debug.LogWarning("Pad prefab not assigned!");
            return;
        }

        if (gridRows == null || gridRows.Count == 0)
        {
            Debug.LogError("GridRows is empty! Cannot spawn pads.");
            return;
        }

        padPositions.Clear();
        padsActivated = 0;

        int height = gridRows.Count;
        int width = gridRows[0].row.Count;

        int tries = 0;
        int maxTries = 200;

        while (padPositions.Count < count && tries < maxTries)
        {
            tries++;
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // Skip invalid tiles
            string code = gridRows[y].row[x];
            if (code == "." || code == "W" || code == "X")
                continue;

            Vector2 newPos = new Vector2(x, y);

            // Check distance constraint
            bool tooClose = false;
            foreach (var existing in padPositions)
            {
                if (Vector2.Distance(existing, newPos) < minTileDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            padPositions.Add(newPos);
        }

        // Spawn pads
        foreach (var tilePos in padPositions)
        {
            // Find the tile GameObject at this position
            Vector3 worldPos = new Vector3(tilePos.x * tileScale, (height - 1 - tilePos.y) * tileScale, 0f);

            Vector3 entranceWorldPos = new Vector3(0, 10, 0f);
            Vector2Int entranceGrid = FindEntrance();
            Vector3 entranceTilePos = new Vector3(entranceGrid.x * tileScale, (height - 1 - entranceGrid.y) * tileScale, 0f);
            Vector3 offset = entranceWorldPos - entranceTilePos;

            worldPos += offset;

            // Find tile object at this world position
            Transform tileTransform = null;
            foreach (Transform child in transform)
            {
                if (Vector3.Distance(child.position, worldPos) < 0.1f) // tolerance
                {
                    tileTransform = child;
                    break;
                }
            }

            if (tileTransform != null)
            {
                GameObject pad = Instantiate(padPrefab, tileTransform.Find("Base"));
                pad.transform.localPosition = Vector3.zero; // centered on tile
                PadActivator activator = pad.AddComponent<PadActivator>();
                activator.tileManager = this;
            }
            else
            {
                Debug.LogWarning($"Could not find tile at {worldPos} for pad.");
            }
        }
    }


    private Vector2Int FindEntrance()
    {
        for (int y = 0; y < gridRows.Count; y++)
        {
            for (int x = 0; x < gridRows[y].row.Count; x++)
            {
                if (gridRows[y].row[x] == "S")
                    return new Vector2Int(x, y);
            }
        }
        return Vector2Int.zero;
    }

    public void PadPressed()
    {
        padsActivated++;
        endObject.GetComponent<EndTile>().OnPadPressed();

        if (padsActivated >= padPositions.Count)
        {
            Debug.Log("All pads activated!");
        }
    }




    public void PopulateGridFromText(TextAsset file)
    {
        gridRows.Clear();

        string[] lines = file.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            string[] codes = trimmedLine.Split(' ');
            TileRow row = new TileRow();
            row.row.AddRange(codes);

            gridRows.Add(row);
        }

        Debug.Log("Grid populated from text. Rows: " + gridRows.Count);
    }
}
