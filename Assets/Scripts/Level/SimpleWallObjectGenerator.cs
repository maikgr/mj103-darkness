using UnityEngine;
using System.Collections.Generic;

public class SimpleWallObjectGenerator : MonoBehaviour
{
    public GameObject WallPrefab;
    public List<Sprite> WallSprites;
    public List<Sprite> WallSpriteMasks;
    public List<Sprite> WallLightMasks;
    public Sprite InnerWallSprite;

    public GameObject GetWallObject(string[] template, int rowIndex, int colIndex)
    {
        var row = template[rowIndex];
        var spriteChild = WallPrefab.GetComponentInChildren<SpriteRenderer>();
        var spriteMaskChild = WallPrefab.GetComponentInChildren<SpriteMask>();
        var rand = Random.Range(0, WallSprites.Count);

        spriteChild.sprite = WallSprites[rand];
        spriteChild.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        spriteMaskChild.sprite = WallSpriteMasks[rand];

        // Top side neighbour
        if (rowIndex > 0 && template[rowIndex - 1][colIndex] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.identity;
        }
        // Left side neighbour
        else if (colIndex > 0 && template[rowIndex][colIndex - 1] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        // Right side neighbour
        else if (colIndex < row.Length - 1 && template[rowIndex][colIndex + 1] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        // Bottom middle neighbour
        else if (rowIndex < template.Length - 1 && template[rowIndex + 1][colIndex] == 'E')
        {
            spriteChild.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            spriteMaskChild.enabled = false;
            spriteChild.maskInteraction = SpriteMaskInteraction.None;
            spriteChild.sprite = InnerWallSprite;
        }

        return WallPrefab;
    }
}