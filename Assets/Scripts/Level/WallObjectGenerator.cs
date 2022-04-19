using System.Collections.Generic;
using UnityEngine;

public class WallObjectGenerator : MonoBehaviour
{
    public GameObject WallBasePrefab;
    public GameObject WallSidePrefab;
    public GameObject WallCornerPrefab;

    // Dictionary index refers to wall neighbours that has lights
    // 0 1 2
    // 3 W 4
    // 5 6 7
    private IDictionary<string, GameObject> wallObjectPool;
    private GameObject poolParent;

    void Start()
    {
        wallObjectPool = new Dictionary<string, GameObject>();
        poolParent = new GameObject(nameof(WallObjectGenerator));
        poolParent.SetActive(false);
    }

    public GameObject GetWallObject(string[] template, int rowIndex, int colIndex)
    {
        var index = GetWallIndex(template, rowIndex, colIndex);
        if (!wallObjectPool.ContainsKey(index))
        {
            var wall = GeneratePoolItem(index);
            wallObjectPool.Add(index, wall);
        }

        return wallObjectPool[index];
    }

    private GameObject GeneratePoolItem(string index)
    {
        var wallAsset = new GameObject(index);

        var wallBase = GameObject.Instantiate(WallBasePrefab, Vector2.zero, Quaternion.identity);
        wallBase.transform.SetParent(wallAsset.transform);
        // Remove corners when the related sides exist
        if (index.Contains("1"))
        {
            index.Replace("0", "");
            index.Replace("2", "");
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("3"))
        {
            index.Replace("0", "");
            index.Replace("5", "");
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.localRotation = Quaternion.Euler(0, 0, 90);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("4"))
        {
            index.Replace("2", "");
            index.Replace("7", "");
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.localRotation = Quaternion.Euler(0, 0, -90);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("6"))
        {
            index.Replace("5", "");
            index.Replace("7", "");
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.localRotation = Quaternion.Euler(0, 0, 180);
            side.transform.SetParent(wallAsset.transform);
        }

        // Add corners
        if (index.Contains("0"))
        {
            var corner = GameObject.Instantiate(WallCornerPrefab, Vector2.zero, Quaternion.identity);
            corner.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("2"))
        {
            var corner = GameObject.Instantiate(WallCornerPrefab, Vector2.zero, Quaternion.identity);
            corner.transform.localRotation = Quaternion.Euler(0, 0, -90);
            corner.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("5"))
        {
            var corner = GameObject.Instantiate(WallCornerPrefab, Vector2.zero, Quaternion.identity);
            corner.transform.localRotation = Quaternion.Euler(0, 0, 90);
            corner.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("7"))
        {
            var corner = GameObject.Instantiate(WallCornerPrefab, Vector2.zero, Quaternion.identity);
            corner.transform.localRotation = Quaternion.Euler(0, 0, 180);
            corner.transform.SetParent(wallAsset.transform);
        }
        wallAsset.transform.SetParent(poolParent.transform);
        return wallAsset;
    }

    private string GetWallIndex(string[] template, int rowIndex, int colIndex)
    {
        var index = "W";
        var row = template[rowIndex];
        var sides = new bool[4] { false, false, false, false}; // top, right, bottom, left
        var corners = new bool[4] { false, false, false, false}; // topleft, topright, bottomright, bottomleft
        if (rowIndex > 0)
        {
            var topRowIndex = rowIndex - 1;
            // Top left neighbour
            if (colIndex > 0 && template[topRowIndex][colIndex - 1] != 'W') {
                corners[0] = true;
                index += "0";
            }
            // Top middle neighbour
            if (template[topRowIndex][colIndex] != 'W')
            {
                sides[0] = true;
                index += "1";
            }
            // Top right neighbour
            if (colIndex < row.Length - 1 && template[topRowIndex][colIndex + 1] != 'W')
            {
                corners[1] = true;
                index += "2";
            }
        }
        // Left side neighbour
        if (colIndex > 0 && template[rowIndex][colIndex - 1] != 'W')
        {
            sides[3] = true;
            index += "3";
        }
        // Right side neighbour
        if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] != 'W')
        {
            sides[1] = true;
            index += "4";
        }
        if (rowIndex < template.Length - 1)
        {
            var bottomRowIndex = rowIndex + 1;
            // Bottom left neighbour
            if (colIndex > 0 && template[bottomRowIndex][colIndex - 1] != 'W') {
                corners[3] = true;
                index += "5";
            }
            // Bottom middle neighbour
            if (template[bottomRowIndex][colIndex] != 'W')
            {
                sides[2] = true;
                index += "6";
            }
            // Bottom right neighbour
            if (colIndex < row.Length - 1 && template[bottomRowIndex][colIndex + 1] != 'W')
            {
                corners[2] = true;
                index += "7";
            }
        }

        return index;
    }
}
