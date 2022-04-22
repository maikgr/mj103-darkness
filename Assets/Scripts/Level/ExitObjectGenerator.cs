using UnityEngine;

public class ExitObjectGenerator : MonoBehaviour
{
    public GameObject ExitPrefab;

    // Check exit neighbours that has path
    // - 0 -
    // 1 X 2
    // - 3 -
    public GameObject GetExitObject(string[] template, int rowIndex, int colIndex)
    {
        var row = template[rowIndex];
        var spriteChild = ExitPrefab.GetComponentInChildren<SpriteRenderer>().gameObject;

        // Top side neighbour
        if (rowIndex > 0 && template[rowIndex - 1][colIndex] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        // Left side neighbour
        else if (colIndex > 0 && template[rowIndex][colIndex - 1] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.identity;
        }
        // Right side neighbour
        else if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        // Bottom middle neighbour
        else if (rowIndex < template.Length - 1 && template[rowIndex + 1][colIndex] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        return ExitPrefab;
    }
}
