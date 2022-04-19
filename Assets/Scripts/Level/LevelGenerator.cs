using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Mj103Scripts.Level;

public class LevelGenerator : MonoBehaviour
{
    public GameObject CompositeShadowParent;
    public GameObject FloorAsset;
    public GameObject PlayerAsset;
    public GameObject ExitAsset;
    public WallObjectGenerator WallObjectGenerator;
    public ExitObjectGenerator ExitObjectGenerator;
    public TextAsset TemplateJson;
    private List<GameObject> generatedAssets = new List<GameObject>();
    private bool isGenerating = false;
    private IEnumerable<RoomTemplate> roomTemplates;

    void Start()
    {
        roomTemplates = JsonConvert.DeserializeObject<IEnumerable<RoomTemplate>>(TemplateJson.text);
        GenerateLevel(3, 3);
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
        GenerateLevel(3, 3);
        isGenerating = false;
    }

    /** Room type
    * 0: full block
    * 1: one entrance, no exit
    * 2: left and right path
    * 3: left, right, and up path
    * 4: left, right, and down path
    * 5: all direction path
    **/
    private void GenerateLevel(int row, int column)
    {
        var levelTemplate = GenerateLevelTemplate(row, column);
        var translatedTemplate = TranslateLevelTemplate(levelTemplate.Item1, levelTemplate.Item2);
        var merged = MergeTemplate(translatedTemplate);
        RenderTemplate(new Vector2(0, 0), merged);
    }

    private (int[][], int) GenerateLevelTemplate(int row, int column)
    {
        var levelTemplate = Enumerable.Range(0, row).Select(_ => Enumerable.Range(0, column).Select(_ => -1).ToArray()).ToArray();
        var startingIndex = GetRandomIndex(row);
        var current = new int[] { 0, startingIndex }; // [row, col]
        var prev = new int[] { current[0], current[1] };
        var next = new int[] { current[0], current[1] };
        var isPathing = true;

        while(isPathing)
        {
            var direction = GetRandomIndex(3); // 0 go left, 1 go right, 2 go down
            var currentTemplate = UnityEngine.Random.Range(2, 6);
            switch(direction)
            {
                case 0: // go left
                    // Reroll if this is the edge
                    if (current[1] == 0) break;

                    // Check if next cell is already filled
                    next = new int[] { current[0], current[1] - 1 };
                    if (levelTemplate[next[0]][next[1]] != -1) break;

                    // If this room was accessed from previous row
                    if (prev[0] < current[0])
                    {
                        var choices = new int[] { 3, 5 };
                        currentTemplate = choices[GetRandomIndex(choices.Length)];
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = currentTemplate;

                    // Move to next index
                    prev = new int[] { current[0], current[1] };
                    current = new int[] { next[0], next[1]};
                    break;
                case 1: // go right
                    // Reroll if this is the edge
                    if (current[1] == column - 1) break;

                    // Check if next cell is already filled
                    next = new int[] { current[0], current[1] + 1 };
                    if (levelTemplate[next[0]][next[1]] != -1) break;
    
                    // If this room was accessed from previous row
                    if (prev[0] < current[0])
                    {
                        var choices = new int[] { 3, 5 };
                        currentTemplate = choices[GetRandomIndex(choices.Length)];
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = currentTemplate;

                    // Move to next index
                    prev = new int[] { current[0], current[1] };
                    current = new int[] { next[0], next[1]};
                    break;
                case 2: // go down
                    next = new int[] { current[0] + 1, current[1] };

                    // Consider pathing finished
                    if (next[0] == row)
                    {
                        isPathing = false;
                    }
                    // Check if next cell is already filled
                    else if (levelTemplate[next[0]][next[1]] != -1) break;

                    // If this room was accessed from previous row
                    if (prev[0] < current[0])
                    {
                        currentTemplate = 5;
                    }
                    // Make sure template can go down
                    else if (currentTemplate != 4 && currentTemplate != 5)
                    {
                        var choices = new int[] { 4, 5 };
                        currentTemplate = choices[GetRandomIndex(choices.Length)];
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = currentTemplate;

                    // Move to next index
                    prev = new int[] { current[0], current[1] };
                    current = new int[] { next[0], next[1]};
                    break;
            }
        }

        for(var i = 0; i < levelTemplate.Length; ++i)
        {
            for(var j = 0; j < levelTemplate[i].Length; ++j)
            {
                if (levelTemplate[i][j] == -1)
                {
                    levelTemplate[i][j] = GetRandomIndex(5);
                }
            }
        }

        return (levelTemplate, startingIndex);
    }

    private string[][][] TranslateLevelTemplate(int[][] levelTemplate, int startingIndex)
    {
        var translatedTemplates = new List<string[][]>();
        for(var rowIdx = 0; rowIdx < levelTemplate.Length; ++rowIdx)
        {
            var row = levelTemplate[rowIdx];
            var transRow = new List<string[]>();
            for(var colIdx = 0; colIdx < row.Length; ++colIdx)
            {
                var rooms = roomTemplates.Single(r => r.Type == row[colIdx]).Templates.ToArray();
                var room = (string[])rooms[GetRandomIndex(rooms.Length)].Clone();
                if (rowIdx == 0 && colIdx == startingIndex)
                {
                    for(var roomIdx = 0; roomIdx < room.Length; ++roomIdx)
                    {
                        var eIndex = room[roomIdx].IndexOf('E');
                        if(eIndex > 0)
                        {
                            var sb = new StringBuilder(room[roomIdx]);
                            sb[eIndex] = 'P';
                            room[roomIdx] = sb.ToString();
                            break;
                        }
                    }
                }
                transRow.Add(room);
            }
            translatedTemplates.Add(transRow.ToArray());
        }
        return translatedTemplates.ToArray();
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

    private void RenderTemplate(Vector2 startingPoint, string[] template)
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
                    var wall = WallObjectGenerator.GetWallObject(template, i, j);
                    if (wall.GetComponentsInChildren<Transform>().Length <= 1)
                    {
                        continue;
                    }
                    GenerateAsset(wall, currentPoint);
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
                    var exit = ExitObjectGenerator.GetExitObject(template, i, j);
                    GenerateAsset(FloorAsset, currentPoint);
                    GenerateAsset(exit, currentPoint);
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
        obj.transform.localRotation = rotation ?? Quaternion.identity;
        obj.transform.SetParent(CompositeShadowParent.transform);
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

}
