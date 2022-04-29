using UnityEngine;
using System.Collections.Generic;

public class WallHandTrapGenerator : MonoBehaviour
{
    public GameObject wallHandTrapPrefab;
    public GameObject poolParent;
    private IDictionary<string, GameObject> trapObjectPool;
    private string baseIndex = "H";

    public GameObject GetWallHandTrap(string[] template, int rowIndex, int colIndex)
    {
        if (trapObjectPool == null)
        {
            trapObjectPool = GenerateTrapPool();
        }

        var index = GetTrapIndex(template, rowIndex, colIndex);
        if (!trapObjectPool.ContainsKey(index))
        {
            return new GameObject("Invalid trap");
        }

        return trapObjectPool[index];
    }
    
    private string GetTrapIndex(string[] template, int rowIndex, int colIndex)
    {
        var row = template[rowIndex];

        // Top side neighbour
        if (rowIndex > 0 && template[rowIndex - 1][colIndex] == 'W')
        {
            return $"{baseIndex}0";
        }
        // Bottom side neighbour
        else if (rowIndex < template.Length - 1 && template[rowIndex + 1][colIndex] == 'W')
        {
            return $"{baseIndex}3";
        }
        // Left side neighbour
        else if (colIndex > 0 && template[rowIndex][colIndex - 1] == 'W')
        {
            return $"{baseIndex}1";
        }
        // Right side neighbour
        else if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] == 'W')
        {
            return $"{baseIndex}2";
        }

        return baseIndex;
    }

    // Check for nearest wall
    // - 0 -
    // 1 H 2
    // - 3 -
    private IDictionary<string, GameObject> GenerateTrapPool()
    {
        var top = GenerateTrapPoolItem(Quaternion.Euler(0, 0, -90), $"{baseIndex}0");
        var left = GenerateTrapPoolItem(Quaternion.Euler(0, 0, 0), $"{baseIndex}1");
        var right = GenerateTrapPoolItem(Quaternion.Euler(0, 0, 180), $"{baseIndex}2");
        var bottom = GenerateTrapPoolItem(Quaternion.Euler(0, 0, 90), $"{baseIndex}3");

        var pool = new Dictionary<string, GameObject>()
        {
            { $"{baseIndex}0", top },
            { $"{baseIndex}1", left },
            { $"{baseIndex}2", right },
            { $"{baseIndex}3", bottom },
        };

        return pool;
    }

    private GameObject GenerateTrapPoolItem(Quaternion rotation, string index)
    {
        var baseTrap = new GameObject("trap-" + index);
        var trapChild = Instantiate(wallHandTrapPrefab, Vector2.zero, Quaternion.identity);
        trapChild.transform.localRotation = rotation;
        trapChild.transform.SetParent(baseTrap.transform);
        baseTrap.transform.SetParent(poolParent.transform);
        return baseTrap;
    }
}
