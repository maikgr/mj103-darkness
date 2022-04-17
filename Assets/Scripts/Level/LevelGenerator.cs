using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject WallAsset;
    public GameObject FloorAsset;
    private List<GameObject> generatedAssets = new List<GameObject>();
    private bool isGenerating = false;

    void Start()
    {
        GenerateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isGenerating)
        {
            isGenerating = true;
            DestroyAssets();
            GenerateLevel();
            isGenerating = false;
        }
    }

    private void GenerateLevel()
    {
        var topLeft = RoomTemplate.TopLeft[GetRandomIndex(RoomTemplate.TopLeft.Count)];
        var topRight = RoomTemplate.TopRight[GetRandomIndex(RoomTemplate.TopRight.Count)];
        var bottomLeft = RoomTemplate.BottomLeft[GetRandomIndex(RoomTemplate.BottomLeft.Count)];
        var bottomRight = RoomTemplate.BottomRight[GetRandomIndex(RoomTemplate.BottomRight.Count)];

        GenerateTemplate(new Vector2(0, 0), topLeft);
        GenerateTemplate(new Vector2(8, 0), topRight);
        GenerateTemplate(new Vector2(0, -6), bottomLeft);
        GenerateTemplate(new Vector2(8, -6), bottomRight);
    }

    private void GenerateTemplate(Vector2 startingPoint, string[] template)
    {
        var currentPoint = new Vector2(startingPoint.x, startingPoint.y);
        foreach(var row in template)
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
            currentPoint.x = startingPoint.x;
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
