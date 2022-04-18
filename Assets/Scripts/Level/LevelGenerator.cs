using System;
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
                    var wall = GenerateWallAsset(template, row, i, j);
                    GenerateAsset(wall.Item1, currentPoint, wall.Item2);
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

    private (GameObject, Quaternion) GenerateWallAsset(string[] template, string row, int templateIndex, int rowIndex)
    {
        var sides = new bool[4] { false, false, false, false}; // top, right, bottom, left
        var corners = new bool[4] { false, false, false, false}; // topleft, topright, bottomright, bottomleft
        if (templateIndex > 0)
        {
            var topRowIndex = templateIndex - 1;
            // Top left neighbour
            if (rowIndex > 0 && template[topRowIndex][rowIndex - 1] != 'W') {
                corners[0] = true;
            }
            // Top right neighbour
            if (rowIndex < row.Length - 1 && template[topRowIndex][rowIndex + 1] != 'W')
            {
                corners[1] = true;
            }
            // Top middle neighbour
            if (template[topRowIndex][rowIndex] != 'W')
            {
                sides[0] = true;
            }
        }
        // Left side neighbour
        if (rowIndex > 0 && template[templateIndex][rowIndex - 1] != 'W')
        {
            sides[3] = true;
        }
        // Right side neighbour
        if (rowIndex < row.Length - 1 && template[templateIndex][rowIndex + 1] != 'W')
        {
            sides[1] = true;
        }
        if (templateIndex < template.Length - 1)
        {
            var bottomRowIndex = templateIndex + 1;
            // Bottom left neighbour
            if (rowIndex > 0 && template[bottomRowIndex][rowIndex - 1] != 'W') {
                corners[3] = true;
            }
            // Bottom right neighbour
            if (rowIndex < row.Length - 1 && template[bottomRowIndex][rowIndex + 1] != 'W')
            {
                corners[2] = true;
            }
            // Bottom middle neighbour
            if (template[bottomRowIndex][rowIndex] != 'W')
            {
                sides[2] = true;
            }
        }

        GameObject wallAsset;
        var rotationSide = Quaternion.identity;
        var rotationCorner = Quaternion.identity;
        var sideCount = sides.Count(_ => _);
        if (sideCount > 0)
        {
            switch(sideCount)
            {
                case 1:
                    wallAsset = Wall1Sides;
                    var index = Array.IndexOf(sides, true);
                    rotationSide = Quaternion.Euler(0, 0, index * -90);
                    break;
                case 2:
                    wallAsset = Wall2Sides;
                    if (sides[0] && sides[1]) rotationSide = Quaternion.Euler(0, 0, -90);
                    if (sides[1] && sides[2]) rotationSide = Quaternion.Euler(0, 0, 180);
                    if (sides[2] && sides[3]) rotationSide = Quaternion.Euler(0, 0, 90);
                    break;
                case 3:
                    wallAsset = Wall3Sides;
                    if (sides[0] && sides[1] && sides[2]) rotationSide = Quaternion.Euler(0, 0, -90);
                    if (sides[1] && sides[2] && sides[3]) rotationSide = Quaternion.Euler(0, 0, 180);
                    if (sides[2] && sides[3] && sides[0]) rotationSide = Quaternion.Euler(0, 0, 90);
                    break;
                case 4:
                default:
                    wallAsset = Wall4Sides;
                    break;
            }
            return (wallAsset, rotationSide);
        }
        else 
        {
            var cornerCount = corners.Count(_ => _);
            switch(cornerCount)
            {
                case 1:
                    wallAsset = Wall1Corners;
                    var index = Array.IndexOf(corners, true);
                    rotationCorner = Quaternion.Euler(0, 0, index * -90);
                    break;
                case 2:
                    wallAsset = Wall2Corners;
                    if (corners[1] && corners[2]) rotationCorner = Quaternion.Euler(0, 0, -90);
                    if (corners[2] && corners[3]) rotationCorner = Quaternion.Euler(0, 0, 180);
                    if (corners[3] && corners[0]) rotationCorner = Quaternion.Euler(0, 0, 90);
                    break;
                case 3:
                    wallAsset = Wall3Corners;
                    if (corners[1] && corners[2] && corners[3]) rotationSide = Quaternion.Euler(0, 0, -90);
                    if (corners[2] && corners[3] && corners[0]) rotationSide = Quaternion.Euler(0, 0, 180);
                    if (corners[3] && corners[0] && corners[1]) rotationSide = Quaternion.Euler(0, 0, 90);
                    break;
                case 4:
                    wallAsset = Wall4Corners;
                    break;
                default:
                    wallAsset = WallInner;
                    break;
            }
            return (wallAsset, rotationCorner);
        }
    }

    private void GenerateAsset(GameObject asset, Vector2 position, Quaternion? rotation = null)
    {
        var obj = GameObject.Instantiate(asset, position, Quaternion.identity);
        obj.transform.localRotation = rotation ?? Quaternion.identity;
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
        return UnityEngine.Random.Range(0, length);
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
