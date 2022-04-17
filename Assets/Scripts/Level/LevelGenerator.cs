using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject WallAsset;
    public GameObject FloorAsset;
    private List<GameObject> generatedAssets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            DestroyAssets();
            GenerateLevel();
        }
    }

    private void GenerateLevel()
    {
        var topLeft = RoomTemplate.TopLeft[GetRandomIndex(RoomTemplate.TopLeft.Count)];
        var topRight = RoomTemplate.TopRight[GetRandomIndex(RoomTemplate.TopRight.Count)];
        var bottomLeft = RoomTemplate.BottomLeft[GetRandomIndex(RoomTemplate.BottomLeft.Count)];
        var bottomRight = RoomTemplate.BottomRight[GetRandomIndex(RoomTemplate.BottomRight.Count)];

        var currentPoint = new Vector2(0, 0);
        foreach(var row in topLeft)
        {
            foreach(var cell in row)
            {
                GameObject asset = null;
                if (cell == 'W')
                {
                    asset = WallAsset;
                }
                else if (cell == 'E')
                {
                    asset = FloorAsset;
                }

                if (asset != null)
                {
                    var obj = GameObject.Instantiate(asset, currentPoint, Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    generatedAssets.Add(obj);
                }

                currentPoint.x += 1;
            }
            currentPoint.x = 0;
            currentPoint.y -= 1;
        }
    }

    private void DestroyAssets()
    {
        foreach(var obj in generatedAssets)
        {
            GameObject.Destroy(obj);
        }
    }

    private int GetRandomIndex(int length)
    {
        return Random.Range(0, length);
    }

    /**
    * W = Wall
    * E = Empty Ground
    **/
    private static class RoomTemplate
    {
        public static List<string[]> TopLeft {
            get
            {
                return new List<string[]>
                {
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWWWWWWW",
                        "WWEEEEEE",
                        "WWEEEEEE",
                        "WWEEEEEE",
                        "WWEEEWWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEEEEEE",
                        "WWEEEEEE",
                        "WWWEEWWE",
                        "WWWWEEWW",
                        "WWWEEWWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEEWWWW",
                        "WWEEEEWW",
                        "WWEEWEEE",
                        "WWEEWEEE",
                        "WWWEEEWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEEEEWW",
                        "WWEEWWWW",
                        "WWEEWWEE",
                        "WWEEEEEE",
                        "WWEEEWWW",
                    },
                };
            }
        }

        public static List<string[]> TopRight {
            get
            {
                return new List<string[]>
                {
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWWWWWWW",
                        "EEEEEEWW",
                        "EEEEEEWW",
                        "EEWWEEWW",
                        "WWWEEEWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "EEWWWWWW",
                        "EEEEEEWW",
                        "EEEEEEWW",
                        "WWWWEEEW",
                        "WWWEEWWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "EEEEEEWW",
                        "EEWWWEEW",
                        "EEWWWEEW",
                        "EEEEEEEW",
                        "WWWEEEEW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "EEWWWWWW",
                        "EEEWWWWW",
                        "EEEEEEWW",
                        "EEEEEEEW",
                        "WWEEEEWW",
                    },
                };
            }
        }
        public static List<string[]> BottomLeft {
            get
            {
                return new List<string[]>
                {
                    new string[]
                    {
                        "WWEEEEWW",
                        "WWWEEEWW",
                        "WWEEEEEE",
                        "WWEEEEEE",
                        "WWWWEEWW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "WWEEEWWW",
                        "WWWEEWWW",
                        "WWWEEEEE",
                        "WWWWWWEE",
                        "WWWWWWWW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "WWEEEEWW",
                        "WWWEEEEW",
                        "WEEEEWWE",
                        "WWEEWWEE",
                        "WWEEEEEE",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "WEEEEEEE",
                        "WWWEEEWW",
                        "WWWEEEEE",
                        "WWWEEEEE",
                        "WWEEEEWW",
                        "WWWWWWWW",
                    },
                };
            }
        }
        public static List<string[]> BottomRight {
            get
            {
                return new List<string[]>
                {
                    new string[]
                    {
                        "EEEEEEWW",
                        "EEWWWWWW",
                        "EEEEEEWW",
                        "EEEWWWWW",
                        "EEWWWWWW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "WWWEEWWW",
                        "EEWWEEWW",
                        "EEEEEEWW",
                        "EEEEEEWW",
                        "EWWWWWWW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "EEEEEEWW",
                        "EEWWEEWW",
                        "EEEEEEWW",
                        "EEEEWWWW",
                        "EEEEEEEW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "EEWEEEWW",
                        "EEWWWEEW",
                        "EWWWWEEW",
                        "EEWWWEEW",
                        "EEEEEEEW",
                        "WWWWWWWW",
                    },
                };
            }
        }
    }
}
