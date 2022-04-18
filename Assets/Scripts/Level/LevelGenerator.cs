using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Wall4Sides;
    public GameObject Wall3Sides;
    public GameObject Wall2Sides;
    public GameObject Wall1Sides;
    public GameObject Wall4Corners;
    public GameObject Wall3Corners;
    public GameObject Wall2Corners;
    public GameObject Wall1Corners;
    public GameObject WallInner;
    public GameObject FloorAsset;
    public GameObject PlayerAsset;
    public GameObject ExitAsset;
    private List<GameObject> generatedAssets = new List<GameObject>();
    private bool isGenerating = false;

    void Start()
    {
        GenerateLevel();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isGenerating)
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        isGenerating = true;
        DestroyAssets();
        GenerateLevel();
        isGenerating = false;
    }

    private void GenerateLevel()
    {
        var topLeft = RoomTemplate.TopLeft[GetRandomIndex(RoomTemplate.TopLeft.Count)];
        var topRight = RoomTemplate.TopRight[GetRandomIndex(RoomTemplate.TopRight.Count)];
        var bottomLeft = RoomTemplate.BottomLeft[GetRandomIndex(RoomTemplate.BottomLeft.Count)];
        var bottomRight = RoomTemplate.BottomRight[GetRandomIndex(RoomTemplate.BottomRight.Count)];
        var merged = MergeTemplate(new string[][] { topLeft, topRight }, new string[][] { bottomLeft, bottomRight });

        GenerateTemplate(new Vector2(0, 0), merged);
    }

    // Each group of template represent row
    private string[] MergeTemplate(params string[][][] templates)
    {
        var result = new List<string[]>();
        foreach(var template in templates)
        {
            List<string> row = new List<string>();
            for(var i = 0; i < template[0].Length; ++i)
            {
                string rowTemp = "";
                for(var j = 0; j < template.Length; ++j)
                {
                    rowTemp += template[j][i];
                }
                row.Add(rowTemp);
            }
            result.Add(row.ToArray());
        }
        return result.SelectMany(_ => _).ToArray();
    }

    private void GenerateTemplate(Vector2 startingPoint, string[] template)
    {
        var currentPoint = new Vector2(startingPoint.x, startingPoint.y);
        for(var i = 0; i < template.Length; ++i)
        {
            var row = template[i];
            for(var j = 0; j < row.Length; ++j)
            {
                var cell = row[j];
                if (cell == 'W')
                {
                    var sideCount = 0;
                    var cornerCount = 0;
                    var rotationSide = Quaternion.identity;
                    var rotationCorner = Quaternion.identity;
                    if (i > 0)
                    {
                        var topRowIndex = i - 1;
                        // Top left neighbour
                        if (j > 0 && template[topRowIndex][j - 1] != 'W') {
                            cornerCount += 1;
                        }
                        // Top right neighbour
                        if (j < row.Length - 1 && template[topRowIndex][j + 1] != 'W')
                        {
                            cornerCount += 1;
                            rotationCorner = Quaternion.Euler(0, 0, -90);
                        }
                        // Top middle neighbour
                        if (template[topRowIndex][j] != 'W')
                        {
                            sideCount += 1;
                        }
                    }
                    // Left side neighbour
                    if (j > 0 && template[i][j - 1] != 'W')
                    {
                        sideCount += 1;
                        rotationSide = Quaternion.Euler(0, 0, 90);
                    }
                    // Right side neighbour
                    if (j < row.Length - 1 && template[i][j + 1] != 'W')
                    {
                        sideCount += 1;
                        rotationSide = Quaternion.Euler(0, 0, -90);
                    }
                    if (i < template.Length - 1)
                    {
                        var bottomRowIndex = i + 1;
                        // Bottom left neighbour
                        if (j > 0 && template[bottomRowIndex][j - 1] != 'W') {
                            cornerCount += 1;
                            rotationCorner = Quaternion.Euler(0, 0, 90);
                        }
                        // Bottom right neighbour
                        if (j < row.Length - 1 && template[bottomRowIndex][j + 1] != 'W')
                        {
                            cornerCount += 1;
                            rotationCorner = Quaternion.Euler(0, 0, 180);
                        }
                        // Bottom middle neighbour
                        if (template[bottomRowIndex][j] != 'W')
                        {
                            sideCount += 1;
                            rotationSide = Quaternion.Euler(0, 0, 180);
                        }
                    }

                    GameObject wallAsset;
                    if (sideCount > 0)
                    {
                        switch(sideCount)
                        {
                            case 1:
                                wallAsset = Wall1Sides;
                                break;
                            case 2:
                                wallAsset = Wall2Sides;
                                break;
                            case 3:
                                wallAsset = Wall3Sides;
                                break;
                            case 4:
                            default:
                                wallAsset = Wall4Sides;
                                break;
                        }
                        GenerateAsset(wallAsset, currentPoint, rotationSide);
                    }
                    else 
                    {
                        switch(cornerCount)
                        {
                            case 1:
                                wallAsset = Wall1Corners;
                                break;
                            case 2:
                                wallAsset = Wall2Corners;
                                break;
                            case 3:
                                wallAsset = Wall3Corners;
                                break;
                            case 4:
                                wallAsset = Wall4Corners;
                                break;
                            default:
                                wallAsset = WallInner;
                                break;
                        }
                        GenerateAsset(wallAsset, currentPoint, rotationCorner);
                    }

                }
                else if (cell == 'E')
                {
                    GenerateAsset(FloorAsset, currentPoint);
                }
                else if (cell == 'P')
                {
                    GenerateAsset(FloorAsset, currentPoint);
                    GenerateAsset(PlayerAsset, currentPoint);
                }
                else if (cell == 'X')
                {
                    GenerateAsset(FloorAsset, currentPoint);
                    GenerateAsset(ExitAsset, currentPoint);
                }

                currentPoint.x += 1;
            }
            currentPoint.x = startingPoint.x;
            currentPoint.y -= 1;
        }
    }

    private void GenerateAsset(GameObject asset, Vector2 position, Quaternion? rotation = null)
    {
        var obj = GameObject.Instantiate(asset, position, Quaternion.identity);
        var sprite = obj.GetComponentInChildren<SpriteRenderer>().transform;
        sprite.localRotation = rotation ?? Quaternion.identity;
        obj.transform.SetParent(gameObject.transform);
        generatedAssets.Add(obj);
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
    * P = Player Spawwn
    * X = Exit
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
                        "WWEPEEEE",
                        "WWEEEEEE",
                        "WWEEEWWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEPEEEE",
                        "WWEEEEEE",
                        "WWWEEWWE",
                        "WWWWEEWW",
                        "WWWEEWWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEPWWWW",
                        "WWEEEEWW",
                        "WWEEWEEE",
                        "WWEEWEEE",
                        "WWWEEEWW",
                    },
                    new string[]
                    {
                        "WWWWWWWW",
                        "WWEEEPWW",
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
                        "EEEEEEEX",
                        "EEEWWWWW",
                        "EEWWWWWW",
                        "WWWWWWWW",
                    },
                    new string[]
                    {
                        "WWWEEWWW",
                        "EEWWEEWW",
                        "EEEEEEWW",
                        "EEEEEEEX",
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
                        "WWWWWWXW",
                    },
                    new string[]
                    {
                        "EEWEEEWW",
                        "EEWWWEEW",
                        "EWWWWEEW",
                        "EEWWWEEX",
                        "EEEEEEEW",
                        "WWWWWWWW",
                    },
                };
            }
        }
    }
}
