using UnityEngine;

public class ExitObjectGenerator : MonoBehaviour
{
    public GameObject ExitPrefab;
    public GameObject ObjectPool;

    // Check exit neighbours that has path
    // - 0 -
    // 1 X 2
    // - 3 -
    public GameObject GetExitObject(string[] template, int rowIndex, int colIndex)
    {
        var row = template[rowIndex];
        GameObject exitParent = new GameObject();
        GameObject exit = null;

        // Top side neighbour
        if (rowIndex > 0 && template[rowIndex - 1][colIndex] == 'E')
        {
            exit = Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity, exitParent.transform);
            exit.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        // Left side neighbour
        else if (colIndex > 0 && template[rowIndex][colIndex - 1] == 'E')
        {
            exit = Instantiate(ExitPrefab, Vector2.zero, Quaternion.identity, exitParent.transform);
        }
        // Right side neighbour
        else if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] == 'E')
        {
            exit = Instantiate(ExitPrefab, Vector2.zero, Quaternion.Euler(0, 0, 180), exitParent.transform);
            exit.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        // Bottom middle neighbour
        else if (rowIndex < template.Length - 1 && template[rowIndex + 1][colIndex] == 'E')
        {
            exit = Instantiate(ExitPrefab, Vector2.zero, Quaternion.Euler(0, 0, 90), exitParent.transform);
            exit.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        if (exit != null)
        {
            exitParent.transform.SetParent(ObjectPool.transform);
        }

        return exitParent;
    }
}
