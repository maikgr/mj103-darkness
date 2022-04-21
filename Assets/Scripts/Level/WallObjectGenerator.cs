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
    private string baseIndex = "W";

    void Awake()
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

        // Remove corners when the related sides exist
        if (index.Contains("1"))
        {
            index = index.Replace("0", string.Empty);
            index = index.Replace("2", string.Empty);
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("3"))
        {
            index = index.Replace("0", string.Empty);
            index = index.Replace("5", string.Empty);
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.localRotation = Quaternion.Euler(0, 0, 90);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("4"))
        {
            index = index.Replace("2", string.Empty);
            index = index.Replace("7", string.Empty);
            var side = GameObject.Instantiate(WallSidePrefab, Vector2.zero, Quaternion.identity);
            side.transform.localRotation = Quaternion.Euler(0, 0, -90);
            side.transform.SetParent(wallAsset.transform);
        }
        if (index.Contains("6"))
        {
            index = index.Replace("5", string.Empty);
            index = index.Replace("7", string.Empty);
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
        if (index != baseIndex)
        {
            var wallBase = GameObject.Instantiate(WallBasePrefab, Vector2.zero, Quaternion.identity);
            wallBase.transform.SetParent(wallAsset.transform);
        }
        wallAsset.transform.SetParent(poolParent.transform);
        return wallAsset;
    }

    private string GetWallIndex(string[] template, int rowIndex, int colIndex)
    {
        var index = baseIndex;
        var row = template[rowIndex];
        if (rowIndex > 0)
        {
            var topRowIndex = rowIndex - 1;
            // Top left neighbour
            if (colIndex > 0 && template[topRowIndex][colIndex - 1] != 'W') {
                index += "0";
            }
            // Top middle neighbour
            if (template[topRowIndex][colIndex] != 'W')
            {
                index += "1";
            }
            // Top right neighbour
            if (colIndex < row.Length - 1 && template[topRowIndex][colIndex + 1] != 'W')
            {
                index += "2";
            }
        }
        // Left side neighbour
        if (colIndex > 0 && template[rowIndex][colIndex - 1] != 'W')
        {
            index += "3";
        }
        // Right side neighbour
        if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] != 'W')
        {
            index += "4";
        }
        if (rowIndex < template.Length - 1)
        {
            var bottomRowIndex = rowIndex + 1;
            // Bottom left neighbour
            if (colIndex > 0 && template[bottomRowIndex][colIndex - 1] != 'W') {
                index += "5";
            }
            // Bottom middle neighbour
            if (template[bottomRowIndex][colIndex] != 'W')
            {
                index += "6";
            }
            // Bottom right neighbour
            if (colIndex < row.Length - 1 && template[bottomRowIndex][colIndex + 1] != 'W')
            {
                index += "7";
            }
        }

        return index;
    }
}
