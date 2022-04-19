using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitObjectGenerator : MonoBehaviour
{
    public GameObject ExitPrefab;

    // Dictionary index refers to exit neighbours that has path
    // - 0 -
    // 1 X 2
    // - 3 -
    private IDictionary<string, GameObject> exitObjectPool;
    private GameObject poolParent;

    void Awake()
    {
        exitObjectPool = new Dictionary<string, GameObject>();
        poolParent = new GameObject(nameof(ExitObjectGenerator));
        poolParent.SetActive(false);
    }

    public GameObject GetExitObject(string[] template, int rowIndex, int colIndex)
    {
        var index = GetExitIndex(template, rowIndex, colIndex);
        if (!exitObjectPool.ContainsKey(index))
        {
            var wall = GeneratePoolItem(index);
            exitObjectPool.Add(index, wall);
        }

        return exitObjectPool[index];
    }

    private string GetExitIndex(string[] template, int rowIndex, int colIndex)
    {
        var row = template[rowIndex];
        if (rowIndex > 0 && template[rowIndex - 1][colIndex] == 'E')
        {
            return "0";
        }
        // Left side neighbour
        if (colIndex > 0 && template[rowIndex][colIndex - 1] == 'E')
        {
            return "1";
        }
        // Right side neighbour
        if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] == 'E')
        {
            return "2";
        }
        // Bottom middle neighbour
        if (rowIndex < template.Length - 1 && template[rowIndex + 1][colIndex] == 'E')
        {
            return "3";
        }

        return "X";
    }

    private GameObject GeneratePoolItem(string index)
    {
        var exitBase = new GameObject(index);
        if (index == "0")
        {
            var asset = GameObject.Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity);
            asset.transform.localRotation = Quaternion.Euler(0, 0, -90);
            asset.transform.SetParent(exitBase.transform);
        }
        else if (index == "1")
        {
            var asset = GameObject.Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity);
            asset.transform.SetParent(exitBase.transform);
        }
        else if (index == "2")
        {
            var asset = GameObject.Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity);
            asset.transform.localRotation = Quaternion.Euler(0, 0, 180);
            asset.transform.SetParent(exitBase.transform);
        }
        else if (index == "3")
        {
            var asset = GameObject.Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity);
            asset.transform.localRotation = Quaternion.Euler(0, 0, 90);
            asset.transform.SetParent(exitBase.transform);
        }
        exitBase.transform.SetParent(poolParent.transform);
        return exitBase;
    }
}
