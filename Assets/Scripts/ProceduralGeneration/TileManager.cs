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
    public TextAsset levelFile; // assign your txt file here
    public float tileScale = 5f; // spacing between tiles
    public List<TileRow> gridRows = new List<TileRow>();
    public TileCodeMapping[] tileMappings;

    private Dictionary<string, GameObject> codeToPrefab;

    private void Awake()
    {
        BuildDictionary();
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
        int width = 0;
        if (height > 0) width = lines[0].Split(' ').Length;

        // center origin
        Vector2Int center = new Vector2Int(width / 2, height / 2);

        for (int y = 0; y < height; y++)
        {
            string[] codes = lines[y].Trim().Split(' ');

            for (int x = 0; x < codes.Length; x++)
            {
                string code = codes[x].Trim();
                if (code == "." || !codeToPrefab.ContainsKey(code))
                    continue; // skip empty

                GameObject prefab = codeToPrefab[code];
                Vector3 pos = new Vector3((x - center.x) * tileScale, (height - 1 - y - center.y) * tileScale, 0f);

                Instantiate(prefab, pos, Quaternion.identity);
            }
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

    private void Start()
    {
        PopulateGridFromText(levelFile);
    }
}
