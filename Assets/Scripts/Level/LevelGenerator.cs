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
    public LevelInstance levelInstance { get; private set; }

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
        var levelTemplate = GenerateLevelTemplatePath(row, column);
        levelTemplate = GenerateBlockingPath(levelTemplate);
        var translatedTemplate = TranslateLevelTemplate(levelTemplate);
        var merged = MergeTemplate(translatedTemplate);
        var borderedTemplate = AddLevelBorder(merged);
        levelInstance = new LevelInstance(merged[0].Length, merged.Length);
        RenderTemplate(new Vector2(0, 0), borderedTemplate);
    }

    private string[] AddLevelBorder(string[] mergedTemplate)
    {
        var bordered = new List<string>();
        var horizontalBorder = string.Join("", Enumerable.Repeat('W', mergedTemplate[0].Length + 2));
        bordered.Add(horizontalBorder);
        for(var i = 0; i < mergedTemplate.Length; ++i)
        {
            bordered.Add("W" + mergedTemplate[i] + "W");
        }
        bordered.Add(horizontalBorder);
        return bordered.ToArray();
    }

    private int[][] GenerateLevelTemplatePath(int row, int column)
    {
        var levelTemplate = Enumerable.Range(0, row).Select(_ => Enumerable.Range(0, column).Select(_ => -1).ToArray()).ToArray();
        var startingIndex = GetRandomIndex(row);
        var current = new int[] { 0, startingIndex }; // [row, col]
        var prev = new int[] { current[0], current[1] };
        var next = new int[] { current[0], current[1] };
        var isPathing = true;
        var pathTemplates = new int[] { 20, 30, 40, 50 };
        var startingTemplates = new int[] { 21, 31, 41, 51 };
        var endingTemplates = new int[] { 22, 32, 42, 52 };
        while(isPathing)
        {
            var direction = GetRandomIndex(3); // 0 go left, 1 go right, 2 go down
            var templateNumber = pathTemplates[GetRandomIndex(pathTemplates.Length)];
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
                        var choices = new int[] { 30, 50 };
                        templateNumber = choices[GetRandomIndex(choices.Length)];
                    }
                    
                    // If this room is a starting room
                    if (current[0] == 0 && current[1] == startingIndex)
                    {
                        templateNumber = templateNumber + 1;
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = templateNumber;

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
                        var choices = new int[] { 30, 50 };
                        templateNumber = choices[GetRandomIndex(choices.Length)];
                    }
    
                    // If this room is a starting room
                    if (current[0] == 0 && current[1] == startingIndex)
                    {
                        templateNumber = templateNumber + 1;
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = templateNumber;

                    // Move to next index
                    prev = new int[] { current[0], current[1] };
                    current = new int[] { next[0], next[1]};
                    break;
                case 2: // go down
                    next = new int[] { current[0] + 1, current[1] };

                    // If this room was accessed from previous row
                    if (prev[0] < current[0])
                    {
                        templateNumber = 50;
                    }
                    // Make sure template can go down
                    else if (templateNumber != 40 && templateNumber != 50)
                    {
                        var choices = new int[] { 40, 50 };
                        templateNumber = choices[GetRandomIndex(choices.Length)];
                    }

                    // If this room is a starting room
                    if (current[0] == 0 && current[1] == startingIndex)
                    {
                        templateNumber = templateNumber + 1;
                    }
                    // Consider pathing finished
                    else if (next[0] == row)
                    {
                        isPathing = false;
                        templateNumber = templateNumber + 2;
                    }

                    // Place template on current index
                    levelTemplate[current[0]][current[1]] = templateNumber;

                    // Move to next index
                    prev = new int[] { current[0], current[1] };
                    current = new int[] { next[0], next[1]};
                    break;
            }
        }
        return levelTemplate;
    }

    private int[][] GenerateBlockingPath(int[][] levelTemplate)
    {
        var topPathNumbers = new int[] { 30, 50 };
        var bottomPathNumbers = new int[] { 40, 50 };
        var sidePathNumbers = new int[] { 20, 21, 22, 30, 31, 32, 40, 41, 42, 50, 51, 52 };
        for(var i = 0; i < levelTemplate.Length; ++i)
        {
            for(var j = 0; j < levelTemplate[i].Length; ++j)
            {
                if (levelTemplate[i][j] != -1) continue;
                // Top neighbour
                if (i > 0 && bottomPathNumbers.Contains(levelTemplate[i - 1][j]))
                {
                    levelTemplate[i][j] = 10;
                }
                // Right neighbour
                else if (j < levelTemplate[i].Length - 1 && sidePathNumbers.Contains(levelTemplate[i][j + 1]))
                {
                    levelTemplate[i][j] = 11;
                }
                // Bottom neighbour
                else if (i < levelTemplate.Length - 1 && topPathNumbers.Contains(levelTemplate[i + 1][j]))
                {
                    levelTemplate[i][j] = 12;
                }
                // Left neighbour
                else if (j > 0 && sidePathNumbers.Contains(levelTemplate[i][j - 1]))
                {
                    levelTemplate[i][j] = 13;
                }
                else
                {
                    levelTemplate[i][j] = 0;
                }
            }
        }
        return levelTemplate;
    }

    private string[][][] TranslateLevelTemplate(int[][] levelTemplate)
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
