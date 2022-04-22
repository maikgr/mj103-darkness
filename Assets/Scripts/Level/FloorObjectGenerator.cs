using System.Collections.Generic;
using UnityEngine;

public class FloorObjectGenerator : MonoBehaviour
{
    public List<Sprite> floorSprites;
    public SpriteRenderer FloorPrefab;
    private int nextIndex = 0;
    
    public GameObject GetFloorObject()
    {
        FloorPrefab.sprite = floorSprites[nextIndex++];
        if (nextIndex == floorSprites.Count)
        {
            nextIndex = 0;
        }
        return FloorPrefab.gameObject;
    }
}
